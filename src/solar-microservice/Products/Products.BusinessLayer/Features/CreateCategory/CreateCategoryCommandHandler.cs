using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Products.ApplicationLayer.Interfaces;
using Products.CoreModels.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.CreateCategory
{
    public class CreateCategoryCommandHandler(IUnitOfWork unitOfWork,IUtilityInterface utilityInterface,ILogger<CreateCategoryCommandHandler> logger) 
        : ICommandHandler<CreateCategoryRequest, CreateCategoryResponse>
    {
        public async Task<CreateCategoryResponse> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            Categories category;

            var isUpdate = request.categoryId.HasValue;

            
            // UPDATE
            
            if (isUpdate)
            {
                category = await unitOfWork.CategoryInterface.GetCategoryById(request.categoryId!.Value, cancellationToken)
                    ?? throw new NotFoundException("Invalid category id.");

                // Check duplicate only if name changed
                if (!string.Equals(category.CategoryName.Trim(),request.categoryName.Trim(),
                        StringComparison.OrdinalIgnoreCase))
                {
                    var exists = await unitOfWork.CategoryInterface
                        .ExistsCategory(request.categoryName.Trim(),request.categoryId.Value,cancellationToken);
                    logger.LogInformation($"Request CategoryId : {request.categoryId}");
                    logger.LogInformation($"Request Name       : '{request.categoryName}'");
                    logger.LogInformation($"Loaded CategoryId  : {category.CategoryId}");
                    logger.LogInformation($"Loaded Name        : '{category.CategoryName}'");

                    if (exists)
                        throw new BadRequestException("Category name already exists.");
                    logger.LogInformation($"Exists = {exists}");

                    category.CategoryName = request.categoryName.Trim();
                    category.Slug = utilityInterface.GenerateSlug(request.categoryName.Trim());
                }
             
                category.Description = request.description.Trim();
                category.CategoryParentId = request.parentCatgoryId;
                category.UpdatedAt = DateTimeOffset.UtcNow;
            }

            // CREATE
           
            else
            {
                var exists = await unitOfWork.CategoryInterface.ExistCategory(request.categoryName,cancellationToken);

                if (exists)
                    throw new BadRequestException("Category name already exists.");

                category = new Categories
                {
                    CategoryName = request.categoryName.Trim(),
                    Description = request.description.Trim(),
                    CategoryParentId = request.parentCatgoryId,
                    Slug = utilityInterface.GenerateSlug(request.categoryName.Trim()),
                    CreatedAt = DateTimeOffset.UtcNow,
                    IsActive = true
                };

                await unitOfWork.CategoryInterface
                    .AddCategory(category, cancellationToken);
            }

          
            // Parent Validation
           
            if (request.parentCatgoryId.HasValue)
            {
                var parent = await unitOfWork.CategoryInterface
                    .GetCategoryById(request.parentCatgoryId.Value,cancellationToken);

                if (parent is null)
                    throw new NotFoundException("Invalid parent category.");
            }

            // Image Upload
          
            if (request.image is not null)
            {
                // Delete old image only during update
                if (!string.IsNullOrWhiteSpace(category.ImageUrl))
                {
                    await utilityInterface.DeleteImage(
                        category.ImageUrl,
                        cancellationToken);
                }

                category.ImageUrl =
                    await utilityInterface.UploadImage(
                        request.image,
                        UploadFolders.Categories,
                        cancellationToken);
            }
            else
            {
                // Only CREATE of root category requires image
                if (!isUpdate &&!request.parentCatgoryId.HasValue)
                {
                    throw new BadRequestException(
                        "Image is required.");
                }
            }

            await unitOfWork.SaveChanges();

            return new CreateCategoryResponse(
                category.CategoryId,
                category.CategoryName,
                category.CreatedAt,
                category.Slug);
        }

    }
    
}
