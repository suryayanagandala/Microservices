using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Products.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.GetProductForCart
{
    public class GetProductForCartHandler
    (ProductDbContext context)
    : IQueryHandler<GetProductForCartQuery, GetProductForCartResult>
    {
        public async Task<GetProductForCartResult> Handle(
            GetProductForCartQuery query,
            CancellationToken cancellationToken)
        {
            var product = await context.Products
                .Include(x => x.ProductImages)
                .FirstOrDefaultAsync(x =>
                    x.ProductId == query.ProductId &&
                    x.IsActive,
                    cancellationToken);

            if (product is null)
                throw new Exception("Product not found.");

            return new GetProductForCartResult(
                product.ProductId,
                product.ProductName,
                product.Price,
                product.IsActive,
                product.ProductImages.Where(x => x.IsActive).Select(x => x.ImageUrl).FirstOrDefault());
        }
    }
}
