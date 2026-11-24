namespace Orders.Infrastructure.Outbox;

/// <summary>
/// A message that must be published after the Orders database transaction commits.
/// </summary>
public sealed class OutboxMessage
{
    public Guid EventId { get; set; }

    public string EventType { get; set; } = string.Empty;

    public string RoutingKey { get; set; } = string.Empty;

    public string Payload { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? ProcessedAt { get; set; }

    public DateTimeOffset? NextAttemptAt { get; set; }

    public int RetryCount { get; set; }

    public string? LastError { get; set; }

    public Guid? LockToken { get; set; }

    public DateTimeOffset? LockedUntil { get; set; }
}
