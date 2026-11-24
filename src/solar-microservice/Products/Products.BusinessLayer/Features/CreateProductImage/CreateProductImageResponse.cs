using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.CreateProductImage
{
    public record CreateProductImageResponse(int imageId,DateTimeOffset createdAt);
}
