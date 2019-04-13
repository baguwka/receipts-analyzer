using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReceiptsCore.EF.Model;

namespace ReceiptsCore
{
    public class DbItemsProvider : IItemsProvider
    {
        public async Task<IReadOnlyCollection<Item>> GetItemsAsync()
        {
            using (var db = new ApplicationContext())
            {
                var items = await db.Items.ToListAsync();
                return items;
            }
        }
    }
}