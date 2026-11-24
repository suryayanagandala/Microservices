using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Products.BusinessLayer.Features.GetProductsForCartQuery;
using Products.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.GetProductsForCart
{
    public class GetProductsForCartHandler(ProductDbContext context) : IQueryHandler<GetProductsForCartQuery.GetProductsForCartQuery, List<GetProductsForCartResponse>>
    {
        public async Task<List<GetProductsForCartResponse>> Handle(GetProductsForCartQuery.GetProductsForCartQuery request, CancellationToken cancellationToken)
        {
            var products = await context.Products.AsNoTracking()
                .Where(x => request.productIds.Contains(x.ProductId) && x.IsActive)
                .Select(x => new GetProductsForCartResponse(x.ProductId, x.ProductName
                , x.Price, x.IsActive, x.ProductImages.Where(x => x.IsActive)
                .Select(x => x.ImageUrl).FirstOrDefault())).ToListAsync();
            return products;
        }
    }
}
