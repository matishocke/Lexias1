using Lexias.Services.ContactUsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Lexias.Services.ContactUsAPI.Data
{
    public class ContactUsDbContext : DbContext
    {
        public ContactUsDbContext(DbContextOptions<ContactUsDbContext> options) : base(options)
        {
        }

        public DbSet<ContactUs> ContactUs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<ContactUs>().HasData(
                new ContactUs
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    Subject = "Inquiry about services",
                    Message = "Hello, I would like more information about your services.",
                    CreatedAt = DateTime.UtcNow.AddDays(-1) // Set CreatedAt to 30 seconds ago for testing
                },
                new ContactUs
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com",
                    Subject = "Support Request",
                    Message = "I'm having an issue with my recent purchase.",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new ContactUs
                {
                    Id = 3,
                    Name = "Michael Johnson",
                    Email = "michael.johnson@example.com",
                    Subject = "General Question",
                    Message = "Could you provide me with the price list?",
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                }
            );
        }
    }
}
