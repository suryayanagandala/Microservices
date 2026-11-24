using Orders.CoreModels.Enums;

namespace Orders.ApplicationLayer.Interfaces;

public interface IOrderNotificationService
{
    Task SendOrderCreatedAsync(
        string userId,
        int orderId,
        decimal totalAmount,
        CancellationToken cancellationToken);

    Task SendOrderStatusUpdatedAsync(
        string userId,
        int orderId,
        OrderStatus status,
        CancellationToken cancellationToken);
}
