using store_api.Core.DTOs.Requests;
using store_api.Core.DTOs.Responses;

namespace store_api.Core.Interfaces.Services
{
    public interface IOrder
    {
        public ResponseWithData<OrderResponseData> InitiateOrder(InitiateOrderRequest request);
        public Response CompleteOrder(CompleteOrderRequest request);
        public ResponseWithData<AuthenticateOrderData> AuthenticateOrder(AuthenticateOrderRequest request);

    }
}
