using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace store_api.Core.Models
{
    public class Category : DictionaryModel
    {
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        [StringLength(100)]
        public string MetaTitle { get; set; }
        [StringLength(100)]
        [JsonIgnore]
        public string Slug { get; set; }
        public string Image { get; set; }

    }
}
