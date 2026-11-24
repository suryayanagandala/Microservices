using BuildingBlocks.Authentication_.Abstractions;
using BuildingBlocks.Contracts.Events;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using BuildingBlocks.Messaging.Contracts;
using BuildingBlocks.Messaging.Topology;
using MediatR;
using Products.ApplicationLayer.Interfaces;
using Products.CoreModels.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.CreateProductImage
{
    public class CreateProductImageCommandHandler : ICommandHandler<CreateProductImageRequest, CreateProductImageResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUtilityInterface utilityInterface;

        private readonly IEventBus eventBus;

        private readonly ICurrentUser currentUser;
        public CreateProductImageCommandHandler(IUnitOfWork unitOfWork, IUtilityInterface utilityInterface,ICurrentUser currentUser,IEventBus eventBus)
        {
            this.unitOfWork = unitOfWork;
            this.utilityInterface = utilityInterface;
            this.eventBus = eventBus;
            this.currentUser= currentUser;
        }
        public async Task<CreateProductImageResponse> Handle(CreateProductImageRequest request, CancellationToken cancellationToken)
        {
            bool update = request.ImageId.HasValue;

            var userId = currentUser.UserId;

            if (userId is null) throw new UnauthorizedException("you don't have any permissions");
            if (update)
            {
                var productImage = await unitOfWork.ProductInterface
                    .ExistProductImage(request.ImageId!.Value, cancellationToken)
                    ?? throw new NotFoundException("Image not found");

                if (!string.IsNullOrWhiteSpace(productImage.ImageUrl))
                {
                    await utilityInterface.DeleteImage(
                        productImage.ImageUrl,
                        cancellationToken);
                }

                var imageUrl = await utilityInterface.UploadImage(
                    request.UploadFiles.First(),
                    UploadFolders.Products,
                    cancellationToken);

                productImage.ImageUrl = imageUrl;
                productImage.UpdatedAt = DateTimeOffset.UtcNow;

                await unitOfWork.SaveChanges(cancellationToken);

                await eventBus.PublishAsync<UpdateProductImageEvent>(new UpdateProductImageEvent(productImage.ProductId, productImage.ImageId, userId,$"{productImage.Product.ProductName} image is updated"),RoutingKeys.ImageUpdated,cancellationToken);

                return new CreateProductImageResponse(
                    productImage.ImageId,
                    productImage.CreatedAt);
            }

            foreach (var file in request.UploadFiles)
            {
                var imageUrl = await utilityInterface.UploadImage(
                    file,
                    UploadFolders.Products,
                    cancellationToken);

                var productImage = new ProductImages
                {
                    ProductId = request.ProductId,
                    ImageUrl = imageUrl,
                    CreatedAt = DateTimeOffset.UtcNow
                };



                await unitOfWork.ProductInterface
                    .CreateProductImage(productImage, cancellationToken);

                await eventBus.PublishAsync<UploadProductImageEvent>(new UploadProductImageEvent(productImage.ProductId, productImage.ImageId, userId, $"{productImage.Product.ProductName} image is updated"), RoutingKeys.ImageCreated, cancellationToken);

            }


            await unitOfWork.SaveChanges(cancellationToken);

            return new CreateProductImageResponse(0, DateTimeOffset.UtcNow);



        }
    }
}
