using Microsoft.AspNetCore.SignalR;
using Orders.ApplicationLayer.Interfaces;
using Orders.CoreModels.Enums;

namespace ORDERS.API.Hubs;

public sealed class OrderNotificationService(IHubContext<OrderHub> hubContext)
    : IOrderNotificationService
{
    public async Task SendOrderCreatedAsync(
        string userId,
        int orderId,
        decimal totalAmount,
        CancellationToken cancellationToken)
    {
        await hubContext.Clients.User(userId).SendAsync(
            "OrderCreated",
            new { OrderId = orderId, TotalAmount = totalAmount },
            cancellationToken);
    }

    public async Task SendOrderStatusUpdatedAsync(
        string userId,
        int orderId,
        OrderStatus status,
        CancellationToken cancellationToken)
    {
        await hubContext.Clients.User(userId).SendAsync(
            "OrderStatusUpdated",
            new { OrderId = orderId, Status = status.ToString() },
            cancellationToken);
    }
}
