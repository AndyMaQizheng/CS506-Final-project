using store_api.Core.DTOs.Requests;
using store_api.Core.DTOs.Responses;

namespace store_api.Core.Interfaces.Services
{
    public interface ICart
    {
        public ResponseWithData<AddToCartData> AddToCart(AddToCartRequest request);
        public ResponseWithData<AddToCartData> AddTBulkToCart(AddBulkCartRequest request);
        public ResponseWithData<RemoveFromCartData> RemoveFromCart(RemoveFromCartRequest request);
        public ResponseWithData<ViewCartDetails> ViewCart(string sessionid);
        public Response SaveCartForLater(SaveForLaterRequest request);

    }
}
