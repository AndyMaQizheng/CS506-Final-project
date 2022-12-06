using Microsoft.EntityFrameworkCore;
using store_api.Core.Contexts;
using store_api.Core.DTOs.Responses;
using System;
using EFCore.BulkExtensions;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace store_api.Core.Repositories
{
    public class BaseRepository<T> where T : class
    {
        protected readonly DatabaseContext _context;

        public BaseRepository(DatabaseContext context)
        {
            _context = context;
        }
        public void Add(T item)
        {
            _context.Add(item);
        }

        public DatabaseContext Context()
        {
            return _context;
        }

        public T Find(int key)
        {
            return _context.Set<T>().Find(key);
        }
        public T Find(long key)
        {
            return _context.Set<T>().Find(key);
        }

        public T Find(string key, object value)
        {
            return _context.Set<T>().AsEnumerable().Where(b => b.GetType().GetProperty(key).Name == key && b.GetType().GetProperty(key).GetValue(b.GetType().GetProperty(key)) == value).FirstOrDefault();

        }

        public IQueryable<T> FindByQuery(string query, object[] parameters)
        {
            return _context.Set<T>().FromSqlRaw(query, parameters);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public void Remove(int key)
        {
            var item = Find(key);
            if (item == null) throw new Exception("Not found!");
            _context.Remove(item);
        }
        public void Remove(long key)
        {
            var item = Find(key);
            if (item == null) throw new Exception("Not found!");
            _context.Remove(item);
        }

        public void Update(T item)
        {
            _context.Update(item);
        }

        public void BulkInsert(IList<T> t)
        {
            _context.BulkInsert(t);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task<List<H>> ExecuteStoredProc<H>(DbCommand command)
        {
            using (command)
            {
                if (command.Connection.State == System.Data.ConnectionState.Closed)
                    command.Connection.Open();
                try
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        return MapToList<H>(reader);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        public List<H> MapToList<H>(DbDataReader dr)
        {
            var objList = new List<H>();
            var props = typeof(H).GetRuntimeProperties();

            var colMapping = dr.GetColumnSchema()
              .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
              .ToDictionary(key => key.ColumnName.ToLower());

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    H obj = Activator.CreateInstance<H>();
                    foreach (var prop in props)
                    {
                        var val =
                          dr.GetValue(colMapping[prop.Name.ToLower()].ColumnOrdinal.Value);
                        prop.SetValue(obj, val == DBNull.Value ? null : val);
                    }
                    objList.Add(obj);
                }
            }
            return objList;
        }
        public DbCommand AddParam(DbCommand cmd, string paramName, object paramValue)
        {
            if (string.IsNullOrEmpty(cmd.CommandText))
                throw new InvalidOperationException(
                  "Call LoadStoredProc before using this method");
            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.Value = paramValue;
            cmd.Parameters.Add(param);
            return cmd;
        }
        public DbCommand LoadSP(DbContext context, string storedProcName)
        {
            var cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = storedProcName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }

        public List<H> ProcessByProcedure<H>(string name, List<KeyValuePair<string, object>> parameters)
        {
            var cmd = LoadSP(_context, name);
            if (parameters.Count > 0)
            {
                foreach (var obj in parameters)
                {
                    cmd = AddParam(cmd, obj.Key, obj.Value);
                }
            }
            Task<List<H>> task = ExecuteStoredProc<H>(cmd);
            return task.Result;

        }


        public PaginatedResponse ProcessPaginatedDataByProcedure<H>(string name, List<KeyValuePair<string, object>> parameters, int? dataposition = 1)
        {
            var cmd = LoadSP(_context, name);
            if (parameters.Count > 0)
            {
                foreach (var obj in parameters)
                {
                    cmd = AddParam(cmd, obj.Key, obj.Value);
                }
            }
            Task<PaginatedResponse> task = ExecutePaginatedStoredProc<H>(cmd, dataposition);
            return task.Result;

        }
        public async Task<PaginatedResponse> ExecutePaginatedStoredProc<H>(DbCommand command, int? dataposition = 1)
        {
            using (command)
            {
                if (command.Connection.State == System.Data.ConnectionState.Closed)
                    command.Connection.Open();
                try
                {
                    var response = new PaginatedResponse { };
                    using (DbDataReader reader = await command.ExecuteReaderAsync())
                    {

                        var page = MapToList<PageInfo>(reader);
                        response.Page = page.FirstOrDefault();

                        reader.NextResult();
                        var data = MapToList<H>(reader);
                        response.Items = data;
                    }
                    return response;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

    }
}
