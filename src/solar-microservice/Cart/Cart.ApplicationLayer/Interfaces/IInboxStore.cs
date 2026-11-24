using BuildingBlocks.Messaging.Contracts;

namespace Cart.ApplicationLayer.Interfaces;

public interface IInboxStore
{
    Task<bool> HasProcessedAsync(Guid eventId, CancellationToken cancellationToken);

    void AddProcessed(IntegrationEvent integrationEvent);
}
