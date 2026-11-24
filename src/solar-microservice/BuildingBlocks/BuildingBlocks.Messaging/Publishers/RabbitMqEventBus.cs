using System.Text;
using System.Text.Json;
using BuildingBlocks.Messaging.Configuration;
using BuildingBlocks.Messaging.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace BuildingBlocks.Messaging.Publishers;

public sealed class RabbitMqEventBus(
    IChannelFactory channelFactory,
    IOptions<RabbitMqSettings> options,
    ILogger<RabbitMqEventBus> logger) : IEventBus
{
    private readonly RabbitMqSettings _options = options.Value;

    public Task PublishAsync<T>(
        T message,
        string routingKey,
        CancellationToken cancellationToken = default)
        where T : IntegrationEvent
    {
        return PublishAsync((IntegrationEvent)message, routingKey, cancellationToken);
    }

    public async Task PublishAsync(
        IntegrationEvent integrationEvent,
        string routingKey,
        CancellationToken cancellationToken = default)
    {
        var channelOptions = new CreateChannelOptions(
            publisherConfirmationsEnabled: true,
            publisherConfirmationTrackingEnabled: true);
        await using var channel = await channelFactory.CreateChannelAsync(channelOptions);

        await channel.ExchangeDeclareAsync(
            exchange: _options.ExchangeName,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);

        var body = Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType()));
        var properties = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent,
            MessageId = integrationEvent.EventId.ToString()
        };

        await channel.BasicPublishAsync(
            exchange: _options.ExchangeName,
            routingKey: routingKey,
            mandatory: true,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Published {Event} -> {RoutingKey}",
            integrationEvent.GetType().Name,
            routingKey);
    }
}
