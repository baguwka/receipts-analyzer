using System.Threading.Tasks;
using CheckReceiptSDK;
using NUnit.Framework;
using Receipts.Core;
using Receipts.Core.Contract.EF.Model;

namespace FNSIntegrationTesting
{
    public class LoginTesting
    {
        private FnsUser _FnsUser;

        [OneTimeSetUp]
        public async Task Setup()
        {
            var usersProvider = new DbFnsTestUserRepository();
            _FnsUser = await usersProvider.GetMainUserAsync();
        }

        [Test]
        public async Task LoginToFns()
        {
            var login = await FNS.LoginAsync(_FnsUser.Username, _FnsUser.Password);
            Assert.That(login.IsSuccess, Is.EqualTo(true));
        }
    }
}