using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReceiptsServer.Receipts
{
    public class GetReceiptsResult
    {
        [JsonProperty("receipts")]
        public IReadOnlyCollection<ReceiptModel> Receipts { get; set; }
    }
}