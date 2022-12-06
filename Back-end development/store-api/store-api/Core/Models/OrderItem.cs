using System.ComponentModel.DataAnnotations.Schema;

namespace store_api.Core.Models
{
    public class OrderItem : NonDictionaryModel
    {
        public long ProductId { get; set; }
        public long OrderId { get; set; }
        [Column(TypeName = "Decimal(18,2")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "Decimal(18,2")]
        public decimal Discount { get; set; }
        public string Sku { get; set; }
        public bool? OnDiscount { get; set; }
    }
}
