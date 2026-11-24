using Orders.CoreModels.Entities;

namespace Orders.ApplicationLayer.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);

    Task<Order?> GetByIdAsync(int orderId, CancellationToken cancellationToken);

    Task<IReadOnlyList<Order>> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken);

    Task<Order?>GetOrderByUserId(string userId,CancellationToken cancellationToken);

    void Update(Order order);

    Task<List<Order>> GetExpiredPendingOrdersAsync(DateTimeOffset expiryTime,CancellationToken cancellationToken = default);
}
