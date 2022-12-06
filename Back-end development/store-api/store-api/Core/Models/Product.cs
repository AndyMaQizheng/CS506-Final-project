using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace store_api.Core.Models
{
    [Index(nameof(Name), Name = "unique_product_name", IsUnique = true)]
    public class Product : NonDictionaryModel
    {
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(150)]
        public string Description { get; set; }
        [Column(TypeName = "Decimal(18,2")]
        public decimal? Discount { get; set; }
        [Column(TypeName = "Decimal(18,2")]
        public decimal Price { get; set; }
        [StringLength(150)]
        public string Summary { get; set; }
        [StringLength(100)]
        public string Sku { get; set; }
        [StringLength(100)]
        public string MetaTitle { get; set; }
        [StringLength(100)]
        public string Slug { get; set; }
        public int Quantity { get; set; }
        public bool Publish { get; set; }
        public bool IsOnDeal { get; set; }
        public DateTime? DealStartsAtTime { get; set; }
        public DateTime? DealEndsAtTime { get; set; }
        public DateTime? DatePublished { get; set; }
        public int? StatusId { get; set; }
        public bool? IsPhysical { get; set; }
        public bool? CanBeDelivered { get; set; }
        public int CurrencyId { get; set; }
    }

    public class ProductDto
    {
        [JsonIgnore]
        public int Total { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }

}
