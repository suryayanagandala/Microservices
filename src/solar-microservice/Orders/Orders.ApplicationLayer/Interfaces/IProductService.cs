using Orders.CoreModels.Dto_s;

namespace Orders.ApplicationLayer.Interfaces;

public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> GetProductsAsync(
        IReadOnlyCollection<int> productIds,
        CancellationToken cancellationToken);
}
