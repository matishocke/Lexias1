using Lexias.Services.ContactUsAPI.Data;

namespace Lexias.Services.ContactUsAPI.Infrastructure.Services
{
    public class MessageCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public MessageCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ContactUsDbContext>();

                    //Delete messages older than 30 days
                    var cutoffDate = DateTime.UtcNow.AddDays(-30);

                    //// Delete messages older than 20 seconds
                    //var cutoffDate = DateTime.UtcNow.AddSeconds(-20);

                    var oldMessages = dbContext.ContactUs.Where(m => m.CreatedAt < cutoffDate);
                    dbContext.ContactUs.RemoveRange(oldMessages);

                    await dbContext.SaveChangesAsync();
                }

                // Run cleanup every day
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);

                //// Run cleanup every 20 seconds for testing
                //await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }

}
