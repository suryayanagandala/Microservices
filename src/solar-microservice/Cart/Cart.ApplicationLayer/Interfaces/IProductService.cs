using Cart.CoreModels.Dto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.ApplicationLayer.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto?> GetProductByIdAsync(int productId,CancellationToken cancellationToken);
        Task<List<ProductDto>> GetProductsAsync(List<int> productIds,CancellationToken cancellationToken);  
    }
}
