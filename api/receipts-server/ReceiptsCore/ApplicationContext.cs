using Microsoft.EntityFrameworkCore;
using Receipts.Core.Contract.EF.Model;

namespace Receipts.Core
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<FnsUser> Users { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptExtended> ReceiptsExtended { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=receipts-analyzer-db;Username=app_main;Password=1qaz@WSX");
        }
    }
}
