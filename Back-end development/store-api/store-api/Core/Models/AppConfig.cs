using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace store_api.Core.Models
{
    [Index(nameof(KeyName), Name = "ux_configz_key_name", IsUnique = true)]
    public class AppConfig : DictionaryModel
    {
        [JsonIgnore]
        public string KeyValue { get; set; }
        public string KeyName { get; set; }
        public string Description { get; set; }
    }
}
