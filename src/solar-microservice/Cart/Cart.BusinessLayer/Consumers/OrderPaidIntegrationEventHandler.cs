using BuildingBlocks.Messaging.Contracts;
using BuildingBlocks.Messaging.Events;
using Cart.ApplicationLayer.Interfaces;
using Microsoft.Extensions.Logging;

namespace Cart.BusinessLayer.Consumers;

public sealed class OrderPaidIntegrationEventHandler(
    IUnitOfWork unitOfWork,
    IInboxStore inboxStore,
    ICartNotificationService cartNotificationService,
    ILogger<OrderPaidIntegrationEventHandler> logger)
    : IIntegrationEventHandler<OrderPaidIntegrationEvent>
{
    public async Task HandleAsync(
        OrderPaidIntegrationEvent message,
        CancellationToken cancellationToken = default)
    {
        if (await inboxStore.HasProcessedAsync(message.EventId, cancellationToken))
        {
            logger.LogInformation(
                "Ignoring duplicate OrderPaid event {EventId} for Order {OrderId}",
                message.EventId,
                message.OrderId);
            return;
        }

        inboxStore.AddProcessed(message);

        var cart = await unitOfWork.ICartRepository.GetCartByUserId(
            message.UserId,
            cancellationToken);

        if (cart is null)
        {
            await unitOfWork.SaveChanges(cancellationToken);
            logger.LogInformation(
                "Recorded OrderPaid event {EventId}; no active cart exists for User {UserId}",
                message.EventId,
                message.UserId);
            return;
        }

        foreach (var item in cart.CartItems.Where(item => item.IsActive))
        {
            item.IsActive = false;
            item.UpdatedAt = DateTimeOffset.UtcNow;
        }

        cart.IsActive = false;

        var cartCount = await unitOfWork.ICartRepository.GetCartCount(message.UserId);

        // The Inbox row and cart changes commit together in this DbContext save.
        await unitOfWork.SaveChanges(cancellationToken);

        await cartNotificationService.SendCartCount(message.UserId, cartCount);

        logger.LogInformation(
            "Recorded OrderPaid event {EventId} and cleared cart for User {UserId}",
            message.EventId,
            message.UserId);
    }
}
