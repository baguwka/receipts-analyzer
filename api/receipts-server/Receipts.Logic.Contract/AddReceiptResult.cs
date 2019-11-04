using Newtonsoft.Json;

namespace Receipts.Logic.Contract
{
    public class AddReceiptResult
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("addedItemsCount")]
        public int AddedItemsCount { get; set; }
    }
}