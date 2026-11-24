using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Consumers
{
    public sealed class RabbitMqHostedService(
    RabbitMqConsumer consumer,
    ILogger<RabbitMqHostedService> logger)
    : BackgroundService
    {
        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            logger.LogInformation("RabbitMQ Consumer Started");

            await consumer.StartAsync(stoppingToken);

            await Task.Delay(
                Timeout.Infinite,
                stoppingToken);
        }
    }
}
