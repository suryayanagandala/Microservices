namespace Cart.Infrastructure.Inbox;

public sealed class InboxMessage
{
    public Guid EventId { get; set; }

    public string EventType { get; set; } = string.Empty;

    public DateTimeOffset ReceivedAt { get; set; }

    public DateTimeOffset ProcessedAt { get; set; }
}
