using BuildingBlocks.Exceptions.Exceptions;
using Cart.ApplicationLayer.Interfaces;
using Cart.CoreModels.Dto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cart.Infrastructure.Repositories
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"/products/cart/{productId}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return  await response.Content.ReadFromJsonAsync<ProductDto>( cancellationToken);
        }

        public async Task<List<ProductDto>> GetProductsAsync(List<int> productIds,CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "products/cart",
                new
                {
                    ProductIds = productIds
                });

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<List<ProductDto>>(cancellationToken)
                ?? [];
        }
    }
}
