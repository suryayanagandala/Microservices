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

namespace Products.BusinessLayer.Features.CreateProduct
{
    public class CreateProductCommandHandler(IUnitOfWork unitOfWork,IUtilityInterface utilityInterface,IEventBus eventBus,ICurrentUser currentUser) : ICommandHandler<CreateProductRequest, CreateProductResponse>
    {
        public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var existCategoryId = await unitOfWork.CategoryInterface.GetCategoryById(request.categoryId,cancellationToken);

            if(existCategoryId is null)
            {
                throw new NotFoundException("Invalid Category");
            }

            bool existProduct = await unitOfWork.ProductInterface.ExistProduct(request.productName,cancellationToken);

            if (existProduct)
            {
                throw new BadRequestException("Already product exists");
            }

            string unit = utilityInterface.GetUnit(request.capacity, request.categoryId);

            var productDto = new Product
            {
                ProductName = request.productName,
                Brand = request.brand,
                Type = request.type,
                CapacityValue = request.capacityValue,
                Capacity = request.capacity,
                Price = request.price,
                WarrantyYears = request.warranty_years,
                Stock = request.stock,
                Description = request.description,
                CategoryId = request.categoryId,
                Unit=unit
            };

            var userId = currentUser.UserId;

            if (userId is null) throw new UnauthorizedException("you don't have any permissions");

            await unitOfWork.ProductInterface.AddProduct(productDto,cancellationToken);

           await unitOfWork.SaveChanges(cancellationToken);

            await eventBus.PublishAsync(new CreateProductEvent(productDto.ProductId,userId,$"{productDto.ProductName }is succesfully added"), RoutingKeys.ProductCreated, cancellationToken);

            return new CreateProductResponse(productDto.ProductId, true, productDto.CreatedAt);
        }
    }
}
