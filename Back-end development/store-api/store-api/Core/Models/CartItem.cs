namespace store_api.Core.Models
{
    public class CartItem : NonDictionaryModel
    {
        public long ProductId { get; set; }
        public long CartId { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal? Discount { get; set; }
    }

    public class CartItemDto
    {
        public long Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductImage { get; set; }
    }

}
