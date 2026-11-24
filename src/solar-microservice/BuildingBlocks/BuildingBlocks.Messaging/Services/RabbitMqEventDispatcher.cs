using BuildingBlocks.Messaging.Contracts;
using BuildingBlocks.Messaging.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Services
{
    public sealed class RabbitMqEventDispatcher(
    ConsumerRegistry registry,
    IServiceProvider serviceProvider,
    ILogger<RabbitMqEventDispatcher> logger)
    : IEventDispatcher
    {
        public async Task DispatchAsync(
            string routingKey,
            string json,
            CancellationToken cancellationToken = default)
        {
            if (!registry.TryGet(routingKey, out var registration))
            {
                logger.LogWarning(
                    "No handler registered for RoutingKey : {RoutingKey}",
                    routingKey);

                return;
            }

            var integrationEvent = JsonSerializer.Deserialize(
                json,
                registration.EventType);

            if (integrationEvent == null)
            {
                logger.LogWarning(
                    "Unable to deserialize message for {RoutingKey}",
                    routingKey);

                return;
            }

            

            using var scope = serviceProvider.CreateScope();

            var invoker = (IEventInvoker)
                scope.ServiceProvider.GetRequiredService(
                    registration.InvokerType);

            await invoker.InvokeAsync(
                integrationEvent,
                cancellationToken);
        }
    }
}
