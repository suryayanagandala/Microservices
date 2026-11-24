using BuildingBlocks.Messaging.Contracts;
using Cart.ApplicationLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cart.Infrastructure.Inbox;

public sealed class InboxStore(CartDbContext dbContext) : IInboxStore
{
    public Task<bool> HasProcessedAsync(
        Guid eventId,
        CancellationToken cancellationToken)
    {
        return dbContext.InboxMessages
            .AnyAsync(message => message.EventId == eventId, cancellationToken);
    }

    public void AddProcessed(IntegrationEvent integrationEvent)
    {
        var eventType = integrationEvent.GetType();

        dbContext.InboxMessages.Add(new InboxMessage
        {
            EventId = integrationEvent.EventId,
            EventType = eventType.FullName ?? eventType.Name,
            ReceivedAt = DateTimeOffset.UtcNow,
            ProcessedAt = DateTimeOffset.UtcNow
        });
    }
}
