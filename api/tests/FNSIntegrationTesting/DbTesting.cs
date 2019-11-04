using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Receipts.Core;

namespace FNSIntegrationTesting
{
    [TestFixture]
    public class DbTesting
    {
        [Test]
        public async Task GetReceiptWithItems()
        {
            var provider = new DbReceiptsRepository();
            var receipts = await provider.GetReceiptsAsync();
            Assert.That(receipts.All(r => r.Items != null), Is.True);
        }
    }
}