using Newtonsoft.Json;
using store_api.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace store_api.Core.DTOs.Requests
{
    public class AddToCartRequest
    {
        public string SessionId { get; set; }
        [Required]
        public long ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [JsonIgnore]
        public string Ip { get; set; }
        [Required]
        [RegularExpression("^(set)|(increase)$", ErrorMessage = "document type can either be set or increase")]
        public string Type { get; set; }
    }

    public class AddBulkCartRequest
    {
        public string SessionId { get; set; }

        public List<CartQuantity> Products { get; set; }
        [JsonIgnore]
        public string Ip { get; set; }
    }

    public class CartQuantity
    {
        [Required]
        public long ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [RegularExpression("^(set)|(increase)$", ErrorMessage = "document type can either be set or increase")]
        public string Type { get; set; }

    }
    public class RemoveFromCartRequest
    {
        [Required]
        public string SessionId { get; set; }
        [Required]
        public long ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [RegularExpression("^(set)|(decrease)$", ErrorMessage = "document type can either be set or decrease")]
        public string Type { get; set; }

    }

    public class SaveForLaterRequest
    {
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SessionId { get; set; }
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Identifier { get; set; }
    }

    public class AddToCartData
    {
        public string SessionId { get; set; }
        public ViewCartDetails CartData { get; set; }
    }

    public class RemoveFromCartData
    {
        public string SessionId { get; set; }
        public ViewCartDetails CartData { get; set; }

    }

    public class ViewCartDetails
    {
        public string TotalAmount { get; set; }

        public List<CartItemDto> Items { get; set; }

    }
}
