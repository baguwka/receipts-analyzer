using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Receipts.Core.Contract;
using Receipts.Core.Contract.EF.Model;

namespace ReceiptsCore
{
    public class DbReceiptsRepository : IReceiptsRepository
    {
        public async Task<IReadOnlyCollection<Receipt>> GetReceiptsAsync()
        {
            using (var db = new ApplicationContext())
            {
                var receipts = await db.Receipts
                    .Include(r => r.Items)
                    .Include(r => r.User)
                    .Include(r => r.Extended)
                    .ToListAsync();
                return receipts;
            }
        }

        public async Task<Receipt> GetReceiptByHashAsync(string hash)
        {
            using (var db = new ApplicationContext())
            {
                var receipt = await db.Receipts
                    .Where(r => string.Equals(r.Hash, hash, StringComparison.Ordinal))
                    .Include(r => r.Items)
                    .Include(r => r.User)
                    .Include(r => r.Extended)
                    .FirstOrDefaultAsync();
                return receipt;
            }
        }

        public async Task<Receipt> AddReceiptAsync(Receipt receipt)
        {
            using (var db = new ApplicationContext())
            {
                var addedReceipt = await db.Receipts.AddAsync(receipt);
                await db.SaveChangesAsync();
                return addedReceipt.Entity;
            }
        }

        public async Task<bool> IsReceiptExistsByHashAsync(string hash)
        {
            using (var db = new ApplicationContext())
            {
                var receipt = await db.Receipts
                    .Where(r => string.Equals(r.Hash, hash, StringComparison.Ordinal))
                    .FirstOrDefaultAsync();

                return receipt != null;
            }
        }

        public async Task<ReceiptExtended> AddExtendedInfoToReceipt(ReceiptExtended extended)
        {
            using (var db = new ApplicationContext())
            {
                var addedExtended = await db.ReceiptsExtended.AddAsync(extended);
                await db.SaveChangesAsync();
                return addedExtended.Entity;
            }
        }
    }
}