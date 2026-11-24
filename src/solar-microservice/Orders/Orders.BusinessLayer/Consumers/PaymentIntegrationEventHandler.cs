using BuildingBlocks.Messaging.Contracts;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.Topology;
using Microsoft.Extensions.Logging;
using Orders.ApplicationLayer.Interfaces;
using Orders.CoreModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.BusinessLayer.Consumers
{
    public class PaymentIntegrationEventHandler(IUnitOfWork unitOfWork,IEventBus eventBus,ILogger<PaymentIntegrationEventHandler> logger) : IIntegrationEventHandler<PaymentCompletedIntegrationEvent>
    {
        public async Task HandleAsync(PaymentCompletedIntegrationEvent message, CancellationToken cancellationToken = default)
        {
            logger.LogInformation(
            "Payment completed received for Order {OrderId}",
            message.OrderId);

            var order = await unitOfWork.Orders.GetByIdAsync(message.OrderId, cancellationToken);

            if (order is null)
            {
                logger.LogWarning("Order {OrderId} not found.",message.OrderId);

                return;
            }

            if (order.Status == OrderStatus.PAID)
            {
                logger.LogInformation(
                    "Order {OrderId} already paid.",
                    message.OrderId);

                return;
            }

            order.Status = OrderStatus.PAID;
            order.UpdatedAt = DateTimeOffset.UtcNow;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Order {OrderId} updated to PAID",order.OrderId);

            await eventBus.PublishAsync(
                new OrderPaidIntegrationEvent(
                    order.OrderId,
                    order.UserId,
                    order.TotalAmount,
                    order.UpdatedAt.Value),
                    RoutingKeys.Orders.OrderPaid,
                cancellationToken);
        }
    }
}
