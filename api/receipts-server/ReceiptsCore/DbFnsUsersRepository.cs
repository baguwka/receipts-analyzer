using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Receipts.Core.Contract;
using Receipts.Core.Contract.EF.Model;

namespace Receipts.Core
{
    public class DbFnsUsersRepository : IFnsUsersRepository
    {
        public async Task<FnsUser> GetMainUserAsync()
        {
            using (var db = new ApplicationContext())
            {
                return await db.Users
                    .SingleOrDefaultAsync(u => u.Key == "main");
            }
        }
    }
}