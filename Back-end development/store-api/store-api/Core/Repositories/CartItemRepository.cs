using store_api.Core.Contexts;
using store_api.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace store_api.Core.Repositories
{
    public class CartItemRepository : BaseRepository<CartItem>
    {
        public CartItemRepository(DatabaseContext context) : base(context)
        {
        }

        public List<CartItem> FindCartItems(long cartid)
        {
            return FindByQuery("select * from cartitems where cartid={0}", new object[] { cartid }).ToList();
        }

        public CartItem AddCartItem(long cartid, decimal amount, long productid, int quantity, string sku, decimal? discount)
        {
            var itm = new CartItem
            {
                CartId = cartid,
                DateCreated = DateTime.UtcNow,
                Price = amount,
                ProductId = productid,
                Quantity = quantity,
                Sku = sku,
                Discount = discount
            };

            Add(itm);
            _context.SaveChanges();

            return itm;
        }

        public List<CartItemDto> GetCartItems(long cartitem)
        {
            var param = new List<KeyValuePair<string, object>> {
                new KeyValuePair<string, object>("@cartitemid", cartitem),
             };

            return ProcessByProcedure<CartItemDto>("SP_V1_GET_CART_ITEMS", param).ToList();
        }

    }
}
