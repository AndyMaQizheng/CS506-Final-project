namespace store_api.Core.Models
{
    [Index(nameof(Name), Name = "unique_customer_status_name", IsUnique = true)]
    public class CustomerStatus : DictionaryModel
    {
        public string Name { get; set; }
    }
}
