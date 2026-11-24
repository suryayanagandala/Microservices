using BuildingBlocks.Messaging.Contracts;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.Topology;
using Microsoft.Extensions.Logging;
using Orders.ApplicationLayer.Interfaces;
using Orders.CoreModels.Entities;
using Orders.CoreModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Orders.BusinessLayer.Consumers
{
    public sealed class CartCheckedOutIntegrationEventHandler(IUnitOfWork unitOfWork,ILogger<CartCheckedOutIntegrationEventHandler> logger
        ,IEventBus eventBus) : IIntegrationEventHandler<CartCheckedOutIntegrationEvent>
    {
        public async  Task HandleAsync(CartCheckedOutIntegrationEvent message, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("CheckOut Event Received for User {UserId}" + message.UserId);

            var order = new Order
            {
                UserId = message.UserId,
                OrderDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Status = OrderStatus.PAYMENT_PENDING,
                DeliveryAddress = message.DeliveryAddress.Trim(),
                PhoneNumber = string.IsNullOrWhiteSpace(message.PhoneNumber)
                ? null
                : message.PhoneNumber.Trim(),
                FullName = string.IsNullOrWhiteSpace(message.FullName)
                ? null
                : message.FullName.Trim(),
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow
            };

            foreach (var orderItems in message.CartItemEventDtos)
            {
                

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = orderItems.ProductId,
                    Quantity = orderItems.Quantity,
                    UnitPrice = orderItems.UnitPrice
                });
            }

            order.TotalAmount = order.OrderItems.Sum(
                item => item.Quantity * item.UnitPrice);

            await unitOfWork.Orders.AddAsync(order, cancellationToken);
            var count=await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Order {OrderId} Created Successfully",order.OrderId);

            logger.LogInformation($"{count} rows inserted into orders tables");

            await eventBus.PublishAsync(
           new OrderCreatedIntegrationEvent(order.OrderId,order.UserId,order.TotalAmount),
           RoutingKeys.Orders.OrderCreated,cancellationToken);
        }
    }
}
