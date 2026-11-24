using System.Text.Json;
using BuildingBlocks.Messaging.Contracts;
using Orders.ApplicationLayer.Interfaces;

namespace Orders.Infrastructure.Outbox;

public sealed class OutboxWriter(OrdersDbContext dbContext) : IOutboxWriter
{
    public void Add(IntegrationEvent integrationEvent, string routingKey)
    {
        var eventType = integrationEvent.GetType();

        dbContext.OutboxMessages.Add(new OutboxMessage
        {
            EventId = integrationEvent.EventId,
            EventType = eventType.AssemblyQualifiedName
                ?? throw new InvalidOperationException(
                    $"Could not determine the event type for {eventType.Name}."),
            RoutingKey = routingKey,
            Payload = JsonSerializer.Serialize(integrationEvent, eventType),
            CreatedAt = integrationEvent.OccuredOn
        });
    }
}
