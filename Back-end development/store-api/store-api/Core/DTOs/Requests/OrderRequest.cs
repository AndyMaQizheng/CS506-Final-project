using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace store_api.Core.DTOs.Requests
{
    public class InitiateOrderRequest
    {
        public string SessionId { get; set; }
        public List<InitiateOrderData> Items { get; set; }
    }

    public class InitiateOrderData
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }

    }

    public class OrderResponseData
    {
        public string OrderReference { get; set; }
        public ViewCartDetails OrderData { get; set; }
    }
    public class CompleteOrderRequest
    {
        [JsonIgnore]
        public long CustomerId { get; set; }
        [Required]
        public string OrderReference { get; set; }
        public string ShippingAddress { get; set; }
    }

    public class AuthenticateOrderRequest
    {
        [Required]
        public string OrderReference { get; set; }
    }

    public class AuthenticateOrderData
    {
        public string Token { get; set; }
    }
}
