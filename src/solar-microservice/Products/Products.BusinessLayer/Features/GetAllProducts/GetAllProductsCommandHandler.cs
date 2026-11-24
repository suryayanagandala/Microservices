using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using MediatR;
using Products.ApplicationLayer.Interfaces;
using Products.CoreModels.Dto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.GetAllProducts
{
    public class GetAllProductsCommandHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetAllProductsRequest, IReadOnlyList<GetProductsDto>>
    {
        public async Task<IReadOnlyList<GetProductsDto>> Handle(GetAllProductsRequest request, CancellationToken cancellationToken)
        {
            var getAllProducts = await unitOfWork.ProductInterface.GetAllProducts(null,cancellationToken);

            if (!getAllProducts.Any())
                throw new NotFoundException("Products not found");

            return getAllProducts;
        }
    }
}
