using Lexias.Services.WarehouseAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Lexias.Services.WarehouseAPI.Data
{
    public class AppDbContextWarehouse : DbContext
    {
        public AppDbContextWarehouse(DbContextOptions<AppDbContextWarehouse> options) : base(options)
        {
        }
        public DbSet<Product> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
