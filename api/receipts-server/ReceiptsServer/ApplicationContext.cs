using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ReceiptsServer.EF.Model;

namespace ReceiptsServer
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=receipts-analyzer-db;Username=app_main;Password=1qaz@WSX");
        }
    }
}