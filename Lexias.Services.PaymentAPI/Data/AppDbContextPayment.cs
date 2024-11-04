using Lexias.Services.PaymentAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Lexias.Services.PaymentAPI.Data
{
    public class AppDbContextPayment : DbContext
    {
        public AppDbContextPayment(DbContextOptions<AppDbContextPayment> options) : base(options)
        {
        }
        public DbSet<Payment> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
