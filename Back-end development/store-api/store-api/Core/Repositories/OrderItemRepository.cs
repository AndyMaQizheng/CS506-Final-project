using store_api.Core.Contexts;
using store_api.Core.Models;
using System.Collections.Generic;

namespace store_api.Core.Repositories
{
    public class OrderItemRepository : BaseRepository<OrderItem>
    {
        public OrderItemRepository(DatabaseContext context) : base(context)
        {
        }

        public void InsertBulkOrderItems(List<OrderItem> items)
        {
            BulkInsert(items);
            _context.SaveChanges();
        }
    }
}
