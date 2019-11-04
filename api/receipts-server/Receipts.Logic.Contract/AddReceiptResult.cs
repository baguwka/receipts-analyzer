using Newtonsoft.Json;

namespace Receipts.Logic.Contract
{
    public class AddReceiptResult
    {
        [JsonProperty("id")]
        public int? Id { get; set; }
        
        [JsonProperty("hash")]
        public string Hash { get; set; }
        
        [JsonProperty("raw")]
        public string RawQRData { get; set; }

        [JsonProperty("addedItemsCount")]
        public int? AddedItemsCount { get; set; }
        
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}