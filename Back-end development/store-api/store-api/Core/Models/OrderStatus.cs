using System.ComponentModel.DataAnnotations;

namespace store_api.Core.Models
{
    public class OrderStatus : DictionaryModel
    {
        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
    }
}
