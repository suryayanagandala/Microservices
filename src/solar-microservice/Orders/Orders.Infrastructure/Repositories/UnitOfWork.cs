using Orders.ApplicationLayer.Interfaces;

namespace Orders.Infrastructure.Repositories;

public sealed class UnitOfWork(
    OrdersDbContext dbContext,
    IOrderRepository orderRepository) : IUnitOfWork
{
    public IOrderRepository Orders { get; } = orderRepository;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
