using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orders.ApplicationLayer.Interfaces;
using Orders.CoreModels.Dto_s;
using Orders.CoreModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.BusinessLayer.BackGroundService
{
    public class OrderCleanupWorker(IServiceScopeFactory serviceScopeFactory,ILogger<OrderCleanupWorker> logger, IOptionsMonitor<OrderCleanupOptions> options) : BackgroundService
    {
        private readonly IOptionsMonitor<OrderCleanupOptions> _options = options;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Order Cleanup Worker Started");

            var setting = _options.CurrentValue;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {

                    using var scope=serviceScopeFactory.CreateScope();

                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    

                    var expiryTime = DateTimeOffset.UtcNow.AddMinutes(-setting.ExpiryMinutes);

                    var orders=await unitOfWork.Orders.GetExpiredPendingOrdersAsync(expiryTime,stoppingToken);

                    if (orders.Count>0)
                    {
                        foreach(var order in orders)
                        {
                            order.Status=OrderStatus.CANCELLED;
                            order.UpdatedAt=DateTimeOffset.UtcNow;
                        }

                        await unitOfWork.SaveChangesAsync(stoppingToken);


                        logger.LogInformation("{Count} expired orders cancelled.",orders.Count);
                    }

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while cleanin up");
                }

                await Task.Delay(TimeSpan.FromMinutes(setting.IntervalMinutes),stoppingToken);
            }
        }
    }
}
