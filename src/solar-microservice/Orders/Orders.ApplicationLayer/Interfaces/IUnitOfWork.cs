namespace Orders.ApplicationLayer.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IOrderRepository Orders { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
