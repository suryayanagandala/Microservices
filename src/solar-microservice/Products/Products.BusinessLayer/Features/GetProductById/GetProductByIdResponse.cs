using Products.CoreModels.Dto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.GetProductById
{
    public record GetProductByIdResponse(IReadOnlyList<GetProductsDto> GetProductsDtos);
}
