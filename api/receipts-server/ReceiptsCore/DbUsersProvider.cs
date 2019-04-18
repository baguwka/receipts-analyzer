using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReceiptsCore.EF.Model;

namespace ReceiptsCore
{
    public class DbUsersProvider : IUsersProvider
    {
        public async Task<FnsUser> GetMainUserAsync()
        {
            using (var db = new ApplicationContext())
            {
                return await db.Users
                    .SingleOrDefaultAsync(u => u.Key == "main");
            }
        }

        public async Task<FnsUser> GetUserForTestsAsync()
        {
            using (var db = new ApplicationContext())
            {
                return await db.Users
                    .SingleOrDefaultAsync(u => u.Key == "tests");
            }
        }
    }
}