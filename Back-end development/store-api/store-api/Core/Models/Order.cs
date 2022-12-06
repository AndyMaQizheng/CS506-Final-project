using System.ComponentModel.DataAnnotations;

namespace store_api.Core.Models
{
    public class Order : NonDictionaryModel
    {
        public long? CustomerId { get; set; }
        [StringLength(200)]
        public string SessionId { get; set; }
        public string ShippingAddress { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderReference { get; set; }
        public decimal? TotalAmount { get; set; }
        public string OrderMessage { get; set; }
        public int CurrencyId { get; set; }
        public string ProductNames { get; set; }
    }
}
