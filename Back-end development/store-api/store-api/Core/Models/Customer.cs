using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace store_api.Core.Models
{
    [Index(nameof(EmailAddress), Name = "unique_customer_email", IsUnique = true)]
    [Index(nameof(MobileNumber), Name = "unique_customer_mobile", IsUnique = true)]
    public class Customer : NonDictionaryModel
    {
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(100)]
        public string EmailAddress { get; set; }
        [StringLength(100)]
        public string MobileNumber { get; set; }
        [StringLength(100)]
        public string Username { get; set; }
        [StringLength(200)]
        public string Password { get; set; }
        [StringLength(500)]
        public string Address { get; set; }
        public string Avatar { get; set; }
        public int StatusId { get; set; }
        public long CountryId { get; set; }
        public DateTime? AccountLockedUntil { get; set; }
        public DateTime? DateLocked { get; set; }
        public int? PasswordTries { get; set; }
        public DateTime? LockedUntilForPassword { get; set; }
    }
}
