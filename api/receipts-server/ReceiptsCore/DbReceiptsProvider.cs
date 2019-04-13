using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReceiptsCore.EF.Model;

namespace ReceiptsCore
{
    public class DbReceiptsProvider : IReceiptsProvider
    {
        public async Task<IReadOnlyCollection<Receipt>> GetReceipts()
        {
            using (var db = new ApplicationContext())
            {
                var receipts = await db.Receipts
                    .Include(r => r.Items)
                    .ToListAsync();
                return receipts;
            }
        }

        public async Task<Receipt> GetReceiptByHash(string hash)
        {
            using (var db = new ApplicationContext())
            {
                var receipt = await db.Receipts
                    .Where(r => string.Equals(r.Hash, hash, StringComparison.Ordinal))
                    .Include(r => r.Items)
                    .FirstOrDefaultAsync();
                return receipt;
            }
        }

        public async Task<Receipt> AddReceipt(Receipt receipt)
        {
            using (var db = new ApplicationContext())
            {
                var addedReceipt = await db.Receipts.AddAsync(receipt);
                await db.SaveChangesAsync();
                return addedReceipt.Entity;
            }
        }
    }
}