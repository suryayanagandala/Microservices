using BuildingBlocks.CQRS;
using Products.CoreModels.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.CreateProductImage
{
    public record CreateProductImageRequest(
    int ProductId,
    IReadOnlyList<UploadFile> UploadFiles,
    int? ImageId
) : ICommand<CreateProductImageResponse>;
}
