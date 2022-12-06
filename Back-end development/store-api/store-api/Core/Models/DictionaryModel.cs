using Newtonsoft.Json;
using store_api.Core.Interfaces;
using System;

namespace store_api.Core.Models
{
    public abstract class DictionaryModel : IModel
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        [JsonIgnore]
        public DateTime? DateDeleted { get; set; }
        [JsonIgnore]
        public long? CreatedBy { get; set; }
        [JsonIgnore]
        public long? UpdatedBy { get; set; }
        [JsonIgnore]
        public long? DeletedBy { get; set; }
    }
}
