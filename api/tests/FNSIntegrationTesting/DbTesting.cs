using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using ReceiptsCore;
using ReceiptsCore.Hash;

namespace FNSIntegrationTesting
{
    [TestFixture]
    public class DbTesting
    {
        [Test]
        public async Task GetReceiptWithItems()
        {
            var provider = new DbReceiptsRepository(new NetCoreHashCodeToMd5Calculator());
            var receipts = await provider.GetReceiptsAsync();
            Assert.That(receipts.All(r => r.Items != null), Is.True);
        }
    }
}