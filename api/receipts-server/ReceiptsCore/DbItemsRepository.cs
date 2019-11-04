using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Receipts.Core.Contract;
using Receipts.Core.Contract.EF.Model;

namespace ReceiptsCore
{
    public class DbItemsRepository : IItemsRepository
    {
        public async Task<IReadOnlyCollection<Item>> GetItemsAsync()
        {
            using (var db = new ApplicationContext())
            {
                var items = await db.Items.ToListAsync();
                return items;
            }
        }

        public async Task<Item> AddItem(Item item)
        {
            using (var db = new ApplicationContext())
            {
                var addedReceipt = await db.Items.AddAsync(item);
                await db.SaveChangesAsync();
                return addedReceipt.Entity;
            }
        }
    }
}