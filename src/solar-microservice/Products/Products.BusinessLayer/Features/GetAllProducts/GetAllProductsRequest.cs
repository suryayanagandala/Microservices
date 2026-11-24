using BuildingBlocks.CQRS;
using Products.CoreModels.Dto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.GetAllProducts
{
    public record GetAllProductsRequest():IQuery<IReadOnlyList<GetProductsDto>>;
    
}
