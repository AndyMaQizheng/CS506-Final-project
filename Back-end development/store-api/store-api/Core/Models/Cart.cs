using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace store_api.Core.Models
{
    [Index(nameof(SessionId), Name = "unique_session_id", IsUnique = true)]
    public class Cart : NonDictionaryModel
    {
        public long CustomerId { get; set; }
        [StringLength(200)]
        public string SessionId { get; set; }
        [StringLength(100)]
        public string IpAddress { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool? SavedForLater { get; set; }
    }
}
