namespace store_api.Core.Models
{
    public class ProductCategory : NonDictionaryModel
    {
        public long ProductId { get; set; }
        public int CategoryId { get; set; }

    }
}
