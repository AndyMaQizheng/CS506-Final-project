using store_api.Core.Configs;
using store_api.Core.Exceptions;
using store_api.Core.Interfaces;
using store_api.Core.Models;
using System;

namespace store_api.Core.Services
{
    public class BaseService
    {
        public readonly IUnitOfWork _work;

        public BaseService(IUnitOfWork work)
        {
            _work = work;
        }

        public string Get(string key)
        {
            var variable = Environment.GetEnvironmentVariable(key);
            if (string.IsNullOrEmpty(variable))
            {
                variable = AppConfiguration.Configurations.Find(x => x.KeyName == key)?.KeyValue;
            }
            return variable;
        }

        public Customer GetCustomer(long customerid)
        {
            var customer = _work.CustomerRepository.Find(customerid);
            if (customer == null) throw new CustomException("Customer does not exist");

            return customer;
        }


    }
}
