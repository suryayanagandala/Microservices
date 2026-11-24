using BuildingBlocks.Authentication_.Abstractions;
using BuildingBlocks.Contracts.Events;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using BuildingBlocks.Messaging.Contracts;
using BuildingBlocks.Messaging.Topology;
using Products.ApplicationLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.DeleteProduct
{
    public class DeleteProductCommandHandler(IUnitOfWork unitOfWork,ICurrentUser currentUser,IEventBus eventBus) : ICommandHandler<DeleteProductRequest, DeleteProductResponse>
    {
        public async Task<DeleteProductResponse> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            var existProduct = await unitOfWork.ProductInterface.GetProductDetails(request.productId, cancellationToken);

            if (existProduct is null)
            {
                throw new NotFoundException("Product not found");
            }

            string reason = request.reason.Trim();

            existProduct.IsActive = false;
            existProduct.DeletedReason= reason;
            existProduct.DeletedAt = DateTimeOffset.UtcNow;

            var userId = currentUser.UserId;

            if (userId is null) throw new UnauthorizedException("you don't have any permissions");
            await eventBus.PublishAsync<DeleteProductEvent>(new DeleteProductEvent(existProduct.ProductId, userId, $"{existProduct.ProductName}deleted sucesfully"), RoutingKeys.ProductDeleted, cancellationToken);

            await unitOfWork.SaveChanges();

            return new DeleteProductResponse(true);
        }
    }
}
