using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Products.ApplicationLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.GetProductById
{
    public class GetProductByIdCommandHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetProductByIdRequest, GetProductByIdResponse>
    {
        public async Task<GetProductByIdResponse> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
        {
            var productResponse = await unitOfWork.ProductInterface.GetAllProducts(request.productId, cancellationToken);

            if (!productResponse.Any())
                throw new NotFoundException("Product not found");

            return new GetProductByIdResponse(productResponse);
        }
    }
}
