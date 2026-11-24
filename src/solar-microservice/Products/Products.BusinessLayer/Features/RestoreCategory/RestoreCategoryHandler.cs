using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using MediatR;
using Products.ApplicationLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.RestoreCategory
{
    public class RestoreCategoryHandler(IUnitOfWork unitOfWork) : ICommandHandler<RestoreCategoryRequest>
    {
        public async Task<Unit> Handle(RestoreCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.CategoryInterface
            .CheckCategoryForRestore(request.categoryId, cancellationToken);

            if (category is null)
                throw new NotFoundException("Category not found");

            // Child category
            if (category.CategoryParentId.HasValue)
            {
                var parentCategory = await unitOfWork.CategoryInterface
                    .GetCategoryById(
                        category.CategoryParentId.Value,
                        cancellationToken);

                if (parentCategory is null || !parentCategory.IsActive)
                {
                    throw new BadRequestException(
                        "Please activate the parent category first.");
                }
            }

            category.IsActive = true;
            category.DeletedAt = null;
            category.DeletedReason = null;
            category.UpdatedAt = DateTimeOffset.UtcNow;

            await unitOfWork.SaveChanges(cancellationToken);


            return Unit.Value;
        }
    }
}
