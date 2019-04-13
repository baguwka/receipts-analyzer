using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReceiptsCore.EF.Model;

namespace ReceiptsCore
{
    public class DbUsersProvider : IUsersProvider
    {
        public async Task<User> GetMainUserAsync()
        {
            using (var db = new ApplicationContext())
            {
                return await db.Users
                    .SingleOrDefaultAsync(u => u.Key == "main");
            }
        }

        public async Task<User> GetUserForTestsAsync()
        {
            using (var db = new ApplicationContext())
            {
                return await db.Users
                    .SingleOrDefaultAsync(u => u.Key == "tests");
            }
        }
    }
}