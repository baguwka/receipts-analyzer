using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReceiptsCore.EF.Model;
using ReceiptsCore.Hash;

namespace ReceiptsCore
{
    public class DbReceiptsRepository : IReceiptsRepository
    {
        private readonly IHashCalculator _HashCalculator;

        public DbReceiptsRepository(IHashCalculator hashCalculator)
        {
            _HashCalculator = hashCalculator ?? throw new ArgumentNullException(nameof(hashCalculator));
        }

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
    }
}