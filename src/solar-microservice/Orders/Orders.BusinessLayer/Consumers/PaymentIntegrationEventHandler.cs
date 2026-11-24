using BuildingBlocks.Messaging.Contracts;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.Topology;
using Microsoft.Extensions.Logging;
using Orders.ApplicationLayer.Interfaces;
using Orders.CoreModels.Enums;

namespace Orders.BusinessLayer.Consumers;

public sealed class PaymentIntegrationEventHandler(
    IUnitOfWork unitOfWork,
    IOutboxWriter outboxWriter,
    ILogger<PaymentIntegrationEventHandler> logger)
    : IIntegrationEventHandler<PaymentCompletedIntegrationEvent>
{
    public async Task HandleAsync(
        PaymentCompletedIntegrationEvent message,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Payment completed received for Order {OrderId}",
            message.OrderId);

        var order = await unitOfWork.Orders.GetByIdAsync(
            message.OrderId,
            cancellationToken);

        if (order is null)
        {
            logger.LogWarning("Order {OrderId} not found.", message.OrderId);
            return;
        }

        if (order.Status == OrderStatus.PAID)
        {
            logger.LogInformation("Order {OrderId} already paid.", message.OrderId);
            return;
        }

        order.Status = OrderStatus.PAID;
        order.UpdatedAt = DateTimeOffset.UtcNow;

        var orderPaidEvent = new OrderPaidIntegrationEvent(
            order.OrderId,
            order.UserId,
            order.TotalAmount,
            order.UpdatedAt.Value);

        outboxWriter.Add(orderPaidEvent, RoutingKeys.Orders.OrderPaid);

        // One SaveChanges call persists both the order update and Outbox row.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Order {OrderId} marked paid and OrderPaid event queued in the Outbox",
            order.OrderId);
    }
}
