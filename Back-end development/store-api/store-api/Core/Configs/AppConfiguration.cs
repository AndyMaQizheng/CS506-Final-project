using Microsoft.EntityFrameworkCore;
using store_api.Core.Contexts;
using store_api.Core.Models;
using System;
using System.Collections.Generic;

namespace store_api.Core.Configs
{
    public static class AppConfiguration
    {
        public static List<AppConfig> Configurations { get; set; } = new List<AppConfig> { };

        public static void GetAppConfiguration(string db)
        {
            // Create Db context 
            try
            {
                var options = new DbContextOptionsBuilder<DatabaseContext>()
                    .UseSqlServer(db)
                    .Options;
                var dbcontext = new DatabaseContext(options);
                var work = new UnitOfWork(dbcontext);
                var config = work.AppConfigRepository.GetAll().ToList();
                Configurations = new List<AppConfig> { new AppConfig { Id = 1 } };
                Configurations.AddRange(config);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static string Get(string key)
        {
            var variable = Environment.GetEnvironmentVariable(key);
            if (string.IsNullOrEmpty(variable))
            {
                variable = Configurations.Find(x => x.KeyName == key)?.KeyValue;
            }
            return variable;
        }
    }
}
