using BuildingBlocks.Messaging.Contracts;
using BuildingBlocks.Messaging.Events;
using Cart.ApplicationLayer.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Consumers
{
    public sealed class OrderPaidIntegrationEventHandler(IUnitOfWork unitOfWork,ICartNotificationService cartNotificationService,
    ILogger<OrderPaidIntegrationEventHandler> logger): IIntegrationEventHandler<OrderPaidIntegrationEvent>
    {
        public async Task HandleAsync(
            OrderPaidIntegrationEvent message,
            CancellationToken cancellationToken = default)
        {
            logger.LogInformation(
                "OrderPaid event received for Order {OrderId}",
                message.OrderId);

           var cart= await unitOfWork.ICartRepository.GetCartByUserId(message.UserId,cancellationToken);

            if (cart is null)
                return;

            foreach (var item in cart.CartItems.Where(x => x.IsActive))
            {
                item.IsActive = false;
                item.UpdatedAt = DateTimeOffset.UtcNow;
            }

            cart.IsActive = false;

            int cartCount = await unitOfWork.ICartRepository.GetCartCount(message.UserId);

            await unitOfWork.SaveChanges(cancellationToken);

            await cartNotificationService.SendCartCount(message.UserId, cartCount);
            
                

            logger.LogInformation(
                "Cart cleared for User {UserId}",
                message.UserId);
        }
    }
}
