using Lexias.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Lexias.Services.OrderAPI.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            //builder.ToTable("Orders");  // Specify table name
            //builder.HasKey(o => o.OrderId);

            // Set precision for TotalAmount to avoid issues with decimals
            builder.Property(o => o.TotalAmount).HasPrecision(18, 2);

            //// Configure relationships with OrderItem
            //builder.HasMany(o => o.OrderItemsList)
            //       .WithOne(oi => oi.Order)
            //       .HasForeignKey(oi => oi.OrderId)
            //       .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
