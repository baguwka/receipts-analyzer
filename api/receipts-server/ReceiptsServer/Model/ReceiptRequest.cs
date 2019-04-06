using Newtonsoft.Json;

namespace ReceiptsServer.Model
{
    public class ReceiptRequest
    {
        [JsonProperty("t")]
        public string T { get; set; }

        [JsonProperty("s")]
        public string S { get; set; }

        [JsonProperty("fn")]
        public string Fn { get; set; }

        [JsonProperty("i")]
        public string I { get; set; }

        [JsonProperty("fp")]
        public string Fp { get; set; }

        [JsonProperty("n")]
        public string N { get; set; }
    }
}