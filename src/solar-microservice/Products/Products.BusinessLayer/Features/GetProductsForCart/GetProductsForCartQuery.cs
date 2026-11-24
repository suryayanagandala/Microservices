using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.GetProductsForCartQuery
{
    public record  GetProductsForCartQuery(List<int> productIds) : IQuery<List<GetProductsForCartResponse>>;


    public record GetProductsForCartResponse(
        int productId,
        string productName,
        decimal price,
        bool isactive,
        string? imageUrl);
}

