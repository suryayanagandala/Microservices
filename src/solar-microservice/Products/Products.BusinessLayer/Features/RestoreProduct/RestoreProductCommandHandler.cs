using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using FluentValidation;
using Products.ApplicationLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.RestoreProduct
{
    public class RestoreProductCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<RestoreProductRequest, RestoreProductResponse>
    {
        public async Task<RestoreProductResponse> Handle(RestoreProductRequest request, CancellationToken cancellationToken)
        {
            var existProduct = await unitOfWork.ProductInterface.GetProductDetails(request.productId, cancellationToken);

            if (existProduct is null)
                throw new NotFoundException("ProductId is not found");

            existProduct.IsActive = true;
            existProduct.UpdatedAt = DateTimeOffset.UtcNow;
            existProduct.DeletedAt = null;
            existProduct.DeletedReason = string.Empty;

            await unitOfWork.SaveChanges(cancellationToken);
            return new RestoreProductResponse
            {
                IsSucess = true
            };
        }

    }
}
