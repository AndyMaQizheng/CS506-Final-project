using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace store_api.Core.Models
{
    public class ProductImage : NonDictionaryModel
    {
        [JsonIgnore]
        public long ProductId { get; set; }
        public bool IsDefault { get; set; }
        [StringLength(200)]
        public string ImagePath { get; set; }
    }
}
