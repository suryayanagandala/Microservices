using Microsoft.EntityFrameworkCore;
using Products.ApplicationLayer.Interfaces;
using Products.CoreModels;
using Products.CoreModels.Dto_s;
using Products.CoreModels.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.DbAccessLayer.Repositories
{
    public class ProductRepository : IProductInterface
    {
        private readonly ProductDbContext _db;

        public ProductRepository(ProductDbContext db)
        {
            _db = db;
        }
        public async Task AddProduct(Product product,CancellationToken cancellationToken)
        {
            await _db.Products.AddAsync(product,cancellationToken);
        }

        public async Task CreateProductImage(ProductImages dto,CancellationToken cancellationToken)
        {
            await _db.Set<ProductImages>().AddAsync(dto,cancellationToken);
        }


       

      

        public async Task<bool> ExistProduct(string productName, CancellationToken cancellationToken)
        {
            return await _db.Set<Product>().AnyAsync(x=>x.ProductName == productName);
        }

        public async Task<ProductImages?> ExistProductImage(int imageId, CancellationToken cancellationToken)
        {
            return await _db.Set<ProductImages>().Include(x=>x.Product).FirstOrDefaultAsync(x => x.ImageId == imageId&&x.IsActive,cancellationToken);
        }

        public async Task<IReadOnlyList<GetProductsDto>> GetAllProducts(int? productId,CancellationToken cancellationToken)
        {
            return await _db.Set<Product>().AsNoTracking().Where(x=>x.IsActive&&(!productId.HasValue||x.ProductId==productId.Value))
                .Select(products=>new GetProductsDto(
                    products.ProductId,
                    products.ProductName,
                    products.Description,
                    products.Price,
                    products.Stock,
                    products.WarrantyYears,
                    new CategoriesDto(
                        products.Category.CategoryId,
                        products.Category.CategoryName,
                        products.Category.Description,
                        products.Category.Slug,
                        products.Category.CategoryParentId.HasValue?products.Category.CategoryParentId.Value:0
                        ),
                    products.ProductImages.Where(x=>x.IsActive)
                    .Select(img=>new ProductImageDto(
                        img.ImageId,
                        img.ImageUrl
                        )).ToList()

                    )).ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetProductDetails(int productId, CancellationToken cancellationToken)
        {
            return await _db.Set<Product>().Where(x => x.ProductId == productId && x.IsActive).FirstOrDefaultAsync();
        }
    }
}
