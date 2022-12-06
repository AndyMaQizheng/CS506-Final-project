using store_api.Core.Contexts;
using store_api.Core.Models;
using System;
using System.Linq;

namespace store_api.Core.Repositories
{
    public class CartRepository : BaseRepository<Cart>
    {
        public CartRepository(DatabaseContext context) : base(context)
        {
        }

        public Cart FindWithSessionId(string sessionid)
        {
            return FindByQuery("select * from carts where sessionid={0}", new object[] { sessionid }).FirstOrDefault();
        }

        public Cart NewCart(string sessionid, DateTime expiry, string ip)
        {
            var cart = new Cart
            {
                SessionId = sessionid,
                ExpiryDate = expiry,
                DateCreated = DateTime.UtcNow,
                IpAddress = ip
            };

            Add(cart);

            _context.SaveChanges();
            return cart;
        }
    }
}
