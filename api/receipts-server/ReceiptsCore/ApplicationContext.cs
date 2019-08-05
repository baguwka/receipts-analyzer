using Microsoft.EntityFrameworkCore;
using ReceiptsCore.EF.Model;

namespace ReceiptsCore
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<FnsUser> Users { get; set; }
        public DbSet<Receipt> Receipts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=receipts-analyzer-db;Username=app_main;Password=1qaz@WSX");
        }
    }
}
