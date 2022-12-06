namespace store_api.Core.Models
{
    public class Currency : DictionaryModel
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Symbol { get; set; }
        public bool Allowed { get; set; }

    }
}
