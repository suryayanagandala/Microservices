using BuildingBlocks.CQRS;
using Products.BusinessLayer.Features.CreateProduct;
using Products.BusinessLayer.Features.CreateProductImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.UpdateProduct
{
    public record UpdateProductRequest(int productId,CreateProductRequest request):ICommand<UpdateProductResponse>;
    
}
