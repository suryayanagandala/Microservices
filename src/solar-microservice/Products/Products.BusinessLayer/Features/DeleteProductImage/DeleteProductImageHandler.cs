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

namespace Products.BusinessLayer.Features.DeleteProductImage
{
    public sealed class DeleteProductImageHandler(IUnitOfWork unitOfWork,ICurrentUser currentUser,IEventBus eventBus) : ICommandHandler<DeleteProductImageRequest, DeleteProductImageResponse>
    {
        public async Task<DeleteProductImageResponse> Handle(DeleteProductImageRequest request, CancellationToken cancellationToken)
        {
            var existImage = await unitOfWork.ProductInterface.ExistProductImage(request.imageId, cancellationToken);

            if (existImage is null) throw new NotFoundException("image url not found");

            var userId = currentUser.UserId;

            if (userId is null) throw new UnauthorizedException("you don't have any permissions");

            existImage.IsActive = false;

            await unitOfWork.SaveChanges(cancellationToken);

            await eventBus.PublishAsync<DeleteProductImageEvent>(new DeleteProductImageEvent(existImage.ProductId,existImage.ImageId,userId,$"{existImage.Product.ProductName} has removed  image "), RoutingKeys.ImageDeleted, cancellationToken);

            return new DeleteProductImageResponse(true);
        }
    }
}
