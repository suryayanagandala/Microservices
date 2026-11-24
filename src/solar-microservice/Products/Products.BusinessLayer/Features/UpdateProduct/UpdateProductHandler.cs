using BuildingBlocks.Authentication_.Abstractions;
using BuildingBlocks.Contracts.Events;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using BuildingBlocks.Messaging.Contracts;
using BuildingBlocks.Messaging.Topology;
using Products.ApplicationLayer.Interfaces;
using Products.CoreModels.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.UpdateProduct
{
    public sealed class UpdateProductHandler(IUnitOfWork unitOfWork,IUtilityInterface utilityInterface,IEventBus eventBus,ICurrentUser currentUser) : ICommandHandler<UpdateProductRequest, UpdateProductResponse>
    {
        public async Task<UpdateProductResponse> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var existCategoryId = await unitOfWork.CategoryInterface.GetCategoryById(request.request.categoryId, cancellationToken);

            if (existCategoryId is null)
            {
                throw new NotFoundException("Invalid Category");
            }

            Product? existProductById = await unitOfWork.ProductInterface.GetProductDetails(request.productId, cancellationToken);

            if (existProductById is null) throw new NotFoundException($"Invalid product id");

            if (!string.Equals(existProductById.ProductName,
                   request.request.productName,
                   StringComparison.OrdinalIgnoreCase))
            {
                bool existProduct = await unitOfWork.ProductInterface
                    .ExistProduct(request.request.productName, cancellationToken);

                if (existProduct)
                {
                    throw new BadRequestException("Already product exists");
                }
            }

            string unit = utilityInterface.GetUnit(request.request.capacity, request.request.categoryId);


            existProductById.ProductName = request.request.productName;
            existProductById.Brand = request.request.brand;
            existProductById.Type = request.request.type;
            existProductById.CapacityValue = request.request.capacityValue;
            existProductById.Capacity = request.request.capacity;
            existProductById.Price = request.request.price;
            existProductById.WarrantyYears = request.request.warranty_years;
            existProductById.Stock = request.request.stock;
            existProductById.Description = request.request.description;
            existProductById.CategoryId = request.request.categoryId;
            existProductById.Unit = unit;
            existProductById.UpdatedAt = DateTimeOffset.UtcNow;

            var returnResponse =new  Product{
               ProductId=existProductById.ProductId,
               ProductName=existProductById.ProductName,
               Brand=existProductById.Brand,
               Type=existProductById.Type,
               Capacity=existProductById.Capacity,
               CapacityValue=existProductById.CapacityValue,
               Stock=existProductById.Stock,
               Description=existProductById.Description,
               CategoryId=existProductById.CategoryId,
               UpdatedAt=existProductById.UpdatedAt,
            };

            var userId = currentUser.UserId;

            if (userId is null) throw new UnauthorizedException("you don't have any permissions");

            await unitOfWork.SaveChanges(cancellationToken);

            await eventBus.PublishAsync(new UpdateProductEvent(existProductById.ProductId, userId, $"{existProductById.ProductName} is updated"), RoutingKeys.ProductUpdated, cancellationToken);

            return new UpdateProductResponse(returnResponse);

        }
    }
}
