using ProductsBackend.Data;
using ProductsBackend.Models;

namespace ProductsBackend.WindowsJob
{
    public class WindowJob : BackgroundService
    {

        ILogger<BackgroundService> _logger;
        IServiceScopeFactory _serviceScopeFactory;

        public WindowJob(ILogger<BackgroundService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000000, stoppingToken);
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                    var products = context.Products.AsQueryable();
                    foreach (Product p in products)
                    {
                        p.Quantity = p.Quantity + 2;
                    }
                    await context.SaveChangesAsync();
                    _logger.LogInformation("runnning");
                }
            }

        }
    }
}
