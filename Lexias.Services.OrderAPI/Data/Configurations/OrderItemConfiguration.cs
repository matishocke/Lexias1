using Lexias.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Lexias.Services.OrderAPI.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            //builder.ToTable("OrderItems");  // Specify table name
            //builder.HasKey(oi => oi.OrderItemId);
            
            // Set precision for Price to avoid issues with decimals
            builder.Property(oi => oi.Price).HasPrecision(18, 2);
        }
    }
}
