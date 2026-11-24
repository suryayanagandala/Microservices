using Products.CoreModels.Dto_s;
using Products.CoreModels.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.ApplicationLayer.Interfaces
{
    public interface IProductInterface
    {
        Task AddProduct(Product product,CancellationToken cancellationToken);

        Task<bool> ExistProduct(string productName,CancellationToken cancellationToken);


        Task<Product?> GetProductDetails(int productId,CancellationToken cancellationToken);

        Task<IReadOnlyList<GetProductsDto>> GetAllProducts(int? productId,CancellationToken cancellationToken);

        Task CreateProductImage(ProductImages productImages,CancellationToken cancellationToken=default);

        Task<ProductImages?> ExistProductImage(int imageId,CancellationToken cancellationToken);

       


    }
}
