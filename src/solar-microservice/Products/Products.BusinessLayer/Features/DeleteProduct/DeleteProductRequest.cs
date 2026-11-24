using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.DeleteProduct
{
    public record DeleteProductRequest(int productId,string reason):ICommand<DeleteProductResponse>;
    
}
