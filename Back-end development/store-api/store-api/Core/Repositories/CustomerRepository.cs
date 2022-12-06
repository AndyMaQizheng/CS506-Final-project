using store_api.Core.Contexts;
using store_api.Core.Helpers;
using store_api.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace store_api.Core.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>
    {
        public CustomerRepository(DatabaseContext context) : base(context)
        {
        }

        public Customer GetCustomer(string identifier)
        {
            return FindByQuery("select * from customers where emailaddress = {0} or mobilenumber = {0}", new object[] { identifier }).FirstOrDefault();
        }

        public List<Customer> GetCustomers()
        {
            return FindByQuery("select * from customers where emailaddress = {0} or mobilenumber = {0}", new object[] { }).ToList();
        }
        public Customer CreateCustomer(string firstname, string lastname, string email, string mobile, string password, long country=1, int? status = null, long adminid = -1)
        {
            Customer user = new Customer { };
            try
            {
                user = new Customer
                {
                    FirstName = firstname,
                    LastName = lastname,
                    EmailAddress = email,
                    MobileNumber = mobile,
                    Password = CommonHelper.HashData(password),
                    DateCreated = DateTime.UtcNow,
                    CountryId = country,
                    CreatedBy = adminid,
                    StatusId = status == null ? 1 : (int)status,
                };
                Add(user);
                Commit();
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
