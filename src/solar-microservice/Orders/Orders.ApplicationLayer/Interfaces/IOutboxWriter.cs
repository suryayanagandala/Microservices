using BuildingBlocks.Messaging.Contracts;

namespace Orders.ApplicationLayer.Interfaces;

public interface IOutboxWriter
{
    void Add(IntegrationEvent integrationEvent, string routingKey);
}
