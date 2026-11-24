using Microsoft.EntityFrameworkCore;
using Orders.ApplicationLayer.Interfaces;
using Orders.CoreModels.Entities;

namespace Orders.Infrastructure.Repositories;

public sealed class OrderRepository(OrdersDbContext dbContext) : IOrderRepository
{
    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        await dbContext.Orders.AddAsync(order, cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(int orderId,CancellationToken cancellationToken)
    {
        return await dbContext.Orders
            .AsNoTracking()
            .Include(order => order.OrderItems)
            .FirstOrDefaultAsync(
                order => order.OrderId == orderId && order.IsActive,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetByUserIdAsync(string userId,CancellationToken cancellationToken)
    {
        return await dbContext.Orders
            .AsNoTracking()
            .Include(order => order.OrderItems)
            .Where(order => order.UserId == userId && order.IsActive)
            .OrderByDescending(order => order.OrderDate)
            .ThenByDescending(order => order.OrderId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Order>> GetExpiredPendingOrdersAsync(DateTimeOffset expiryTime, CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders.Where(x => x.IsActive && x.Status == CoreModels.Enums.OrderStatus.PAYMENT_PENDING && x.CreatedAt <= expiryTime).ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetOrderByUserId(string userId, CancellationToken cancellationToken)
    {
        return await dbContext.Orders.Include(order => order.OrderItems)
            .Where(order => order.UserId == userId && order.IsActive).FirstOrDefaultAsync();
    }

    public void Update(Order order)
    {
        dbContext.Orders.Update(order);
    }
}
