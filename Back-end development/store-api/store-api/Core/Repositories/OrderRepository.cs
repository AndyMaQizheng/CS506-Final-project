using store_api.Core.Contexts;
using store_api.Core.DTOs.Responses;
using store_api.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace store_api.Core.Repositories
{
    public class OrderRepository : BaseRepository<Order>
    {
        public OrderRepository(DatabaseContext context) : base(context)
        {

        }

        public Order FindByReference(string reference)
        {
            return FindByQuery("select * from orders where orderreference={0}", new object[] { reference }).FirstOrDefault();
        }

        public Order NewOrder(string sessionid)
        {
            var order = new Order
            {
                SessionId = sessionid,
                DateCreated = DateTime.UtcNow,
                OrderStatusId = 1
            };

            Add(order);

            _context.SaveChanges();
            return order;
        }

    }
}
