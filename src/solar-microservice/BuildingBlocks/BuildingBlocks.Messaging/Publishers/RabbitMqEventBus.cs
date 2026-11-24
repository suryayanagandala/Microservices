using BuildingBlocks.Messaging.Configuration;
using BuildingBlocks.Messaging.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Publishers
{
    public sealed class RabbitMqEventBus(IChannelFactory channelFactory,IOptions<RabbitMqSettings> options,
              ILogger<RabbitMqEventBus> logger): IEventBus
    {
        private readonly RabbitMqSettings _options = options.Value;

        public async Task PublishAsync<T>(T integrationEvent,string routingKey,
            CancellationToken cancellationToken = default)where T : IntegrationEvent
        {
            var channel = await channelFactory.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: _options.ExchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                cancellationToken: cancellationToken);

            var body = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(integrationEvent));

            await channel.BasicPublishAsync(
                exchange: _options.ExchangeName,
                routingKey: routingKey,
                mandatory: true,
                body: body,
                cancellationToken: cancellationToken);

            logger.LogInformation(
                "Published {Event} -> {RoutingKey}",
                typeof(T).Name,
                routingKey);

            await channel.DisposeAsync();
        }
    }
}
