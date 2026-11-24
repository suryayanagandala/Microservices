namespace BuildingBlocks.Messaging.Contracts;

public interface IEventBus
{
    Task PublishAsync<T>(
        T message,
        string routingKey,
        CancellationToken cancellationToken = default)
        where T : IntegrationEvent;

    Task PublishAsync(
        IntegrationEvent message,
        string routingKey,
        CancellationToken cancellationToken = default);
}
