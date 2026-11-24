using System.Text.Json;
using BuildingBlocks.Messaging.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Orders.Infrastructure.Outbox;

public sealed class OutboxPublisher(
    IServiceScopeFactory scopeFactory,
    ILogger<OutboxPublisher> logger) : BackgroundService
{
    private const int BatchSize = 20;
    private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(2);
    private static readonly TimeSpan LeaseDuration = TimeSpan.FromMinutes(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Orders Outbox publisher started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingMessagesAsync(stoppingToken);
                await Task.Delay(PollInterval, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Orders Outbox publisher loop failed");
                await Task.Delay(PollInterval, stoppingToken);
            }
        }
    }

    private async Task ProcessPendingMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
        var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
        var now = DateTimeOffset.UtcNow;

        var pendingIds = await dbContext.OutboxMessages
            .AsNoTracking()
            .Where(message =>
                message.ProcessedAt == null &&
                (message.NextAttemptAt == null || message.NextAttemptAt <= now) &&
                (message.LockedUntil == null || message.LockedUntil <= now))
            .OrderBy(message => message.CreatedAt)
            .Select(message => message.EventId)
            .Take(BatchSize)
            .ToListAsync(cancellationToken);

        foreach (var eventId in pendingIds)
        {
            var lockToken = Guid.NewGuid();
            var leaseExpiresAt = DateTimeOffset.UtcNow.Add(LeaseDuration);

            var claimed = await dbContext.OutboxMessages
                .Where(message =>
                    message.EventId == eventId &&
                    message.ProcessedAt == null &&
                    (message.NextAttemptAt == null || message.NextAttemptAt <= DateTimeOffset.UtcNow) &&
                    (message.LockedUntil == null || message.LockedUntil <= DateTimeOffset.UtcNow))
                .ExecuteUpdateAsync(
                    setters => setters
                        .SetProperty(message => message.LockToken, lockToken)
                        .SetProperty(message => message.LockedUntil, leaseExpiresAt),
                    cancellationToken);

            if (claimed == 0)
            {
                continue;
            }

            var outboxMessage = await dbContext.OutboxMessages
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    message => message.EventId == eventId && message.LockToken == lockToken,
                    cancellationToken);

            if (outboxMessage is null)
            {
                continue;
            }

            try
            {
                var integrationEvent = Deserialize(outboxMessage);

                await eventBus.PublishAsync(
                    integrationEvent,
                    outboxMessage.RoutingKey,
                    cancellationToken);

                var publishedAt = DateTimeOffset.UtcNow;

                await dbContext.OutboxMessages
                    .Where(message =>
                        message.EventId == eventId &&
                        message.LockToken == lockToken)
                    .ExecuteUpdateAsync(
                        setters => setters
                            .SetProperty(message => message.ProcessedAt, publishedAt)
                            .SetProperty(message => message.LockToken, (Guid?)null)
                            .SetProperty(message => message.LockedUntil, (DateTimeOffset?)null)
                            .SetProperty(message => message.LastError, (string?)null),
                        cancellationToken);
            }
            catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
            {
                var retryCount = outboxMessage.RetryCount + 1;
                var delaySeconds = Math.Min(Math.Pow(2, Math.Min(retryCount, 8)), 300);
                var nextAttemptAt = DateTimeOffset.UtcNow.AddSeconds(delaySeconds);
                var error = exception.Message.Length <= 4000
                    ? exception.Message
                    : exception.Message[..4000];

                await dbContext.OutboxMessages
                    .Where(message =>
                        message.EventId == eventId &&
                        message.LockToken == lockToken)
                    .ExecuteUpdateAsync(
                        setters => setters
                            .SetProperty(message => message.RetryCount, retryCount)
                            .SetProperty(message => message.NextAttemptAt, nextAttemptAt)
                            .SetProperty(message => message.LastError, error)
                            .SetProperty(message => message.LockToken, (Guid?)null)
                            .SetProperty(message => message.LockedUntil, (DateTimeOffset?)null),
                        cancellationToken);

                logger.LogWarning(
                    exception,
                    "Outbox event {EventId} failed; retry {RetryCount} scheduled for {NextAttemptAt}",
                    eventId,
                    retryCount,
                    nextAttemptAt);
            }
        }
    }

    private static IntegrationEvent Deserialize(OutboxMessage message)
    {
        var eventType = Type.GetType(message.EventType, throwOnError: false);

        if (eventType is null || !typeof(IntegrationEvent).IsAssignableFrom(eventType))
        {
            throw new InvalidOperationException(
                $"Unknown integration event type '{message.EventType}'.");
        }

        return JsonSerializer.Deserialize(message.Payload, eventType) as IntegrationEvent
            ?? throw new InvalidOperationException(
                $"Could not deserialize Outbox event {message.EventId}.");
    }
}
