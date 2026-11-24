using System.Net.Http.Json;
using Orders.ApplicationLayer.Interfaces;
using Orders.CoreModels.Dto_s;

namespace Orders.Infrastructure.Services;

public sealed class ProductService(HttpClient httpClient) : IProductService
{
    public async Task<IReadOnlyList<ProductDto>> GetProductsAsync(
        IReadOnlyCollection<int> productIds,
        CancellationToken cancellationToken)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "products/cart",
            new { ProductIds = productIds },
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<ProductDto>>(
                   cancellationToken)
               ?? [];
    }
}
