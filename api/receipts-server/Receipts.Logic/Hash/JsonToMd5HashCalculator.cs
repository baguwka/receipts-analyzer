using Newtonsoft.Json;
using Receipts.Logic.Contract.Hash;
using ReceiptsCore.Utils;

namespace Receipts.Logic.Hash
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