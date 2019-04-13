using System.Threading.Tasks;
using CheckReceiptSDK;
using NUnit.Framework;
using ReceiptsCore;
using ReceiptsCore.EF.Model;

namespace FNSIntegrationTesting
{
    public class LoginTesting
    {
        private User _User;

        [OneTimeSetUp]
        public async Task Setup()
        {
            var usersProvider = new DbUsersProvider();
            _User = await usersProvider.GetUserForTestsAsync();
        }

        [Test]
        public async Task LoginToFns()
        {
            var login = await FNS.LoginAsync(_User.Username, _User.Password);
            Assert.That(login.IsSuccess, Is.EqualTo(true));
        }
    }
}