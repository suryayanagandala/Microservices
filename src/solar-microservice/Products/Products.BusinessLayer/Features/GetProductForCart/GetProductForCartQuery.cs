using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.GetProductForCart
{
    public record GetProductForCartQuery(int ProductId)
    : IQuery<GetProductForCartResult>;

    public record GetProductForCartResult(
        int ProductId,
        string ProductName,
        decimal Price,
        bool IsActive,
        string? ImageUrl);
}
