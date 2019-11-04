using System.Collections.Generic;
using Newtonsoft.Json;

namespace Receipts.Logic.Contract.Receipts
{
    public class GetReceiptsResult
    {
        [JsonProperty("receipts")]
        public IReadOnlyCollection<ReceiptModel> Receipts { get; set; }
    }
}