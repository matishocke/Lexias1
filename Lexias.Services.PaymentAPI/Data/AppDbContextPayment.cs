using Lexias.Services.PaymentAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Enum;

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




            modelBuilder.Entity<Payment>().HasData(
                new Payment
                {
                    PaymentId = "payment1",
                    OrderId = "order1111",
                    Status = PaymentStatus.Completed,
                    PaymentDate = DateTime.UtcNow.AddDays(-10),
                    Amount = 50.00M
                },
                new Payment
                {
                    PaymentId = "payment2",
                    OrderId = "order2222",
                    Status = PaymentStatus.Pending,
                    PaymentDate = DateTime.UtcNow.AddDays(-5),
                    Amount = 100.00M
                },
                new Payment
                {
                    PaymentId = "payment3",
                    OrderId = "order3333",
                    Status = PaymentStatus.Failed,
                    PaymentDate = DateTime.UtcNow.AddDays(-2),
                    Amount = 75.00M
                }
            );
        }
    }
}
