using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.GetProductById
{
    public record GetProductByIdRequest(int productId):IQuery<GetProductByIdResponse>;
    
}
