using Newtonsoft.Json;
using ReceiptsCore.Utils;

namespace ReceiptsCore.Hash
{
    public class JsonToMd5HashCalculator : IHashCalculator
    {
        public string Calculate(IHashable hashable)
        {
            var json = JsonConvert.SerializeObject(hashable, Newtonsoft.Json.Formatting.None);
            return json.ToMd5();
        }
    }
}