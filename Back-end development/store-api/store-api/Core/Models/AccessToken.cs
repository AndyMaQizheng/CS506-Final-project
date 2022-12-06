namespace store_api.Core.Models
{
    public class AccessToken : NonDictionaryModel
    {
        public string Token { get; set; }
        public long CustomerId { get; set; }
        public string RefreshToken { get; set; }
    }
}
