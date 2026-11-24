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

namespace Products.BusinessLayer.Features.DeleteCategory
{
    public class DeleteCategoryHandler(IUnitOfWork unitOfWork,IEventBus eventBus,ICurrentUser currentUser) : ICommandHandler<DeleteCategoryRequest, DeleteCategoryResponse>
    {
        public async Task<DeleteCategoryResponse> Handle(
    DeleteCategoryRequest request,CancellationToken cancellationToken)
        {
            var category = await unitOfWork.CategoryInterface
                .GetCategoryById(request.categoryId, cancellationToken);

            if (category is null)
                throw new NotFoundException("Category not found");

            // Root category
            if (!category.CategoryParentId.HasValue)
            {
                if (category.ChildCategories.Any(x => x.IsActive))
                {
                    throw new BadRequestException(
                        "Please deactivate subcategories first.");
                }
            }

            // Check active products
            if (category.Products.Any(x => x.IsActive))
            {
                throw new BadRequestException(
                    "Please deactivate products first.");
            }

            category.IsActive = false;
            category.DeletedReason = request.reason.Trim();
            category.DeletedAt = DateTimeOffset.UtcNow;
            category.UpdatedAt = DateTimeOffset.UtcNow;

            await unitOfWork.SaveChanges(cancellationToken);

            var userId = currentUser.UserId;

            if (userId is null) throw new UnauthorizedException("you don't have any permissions");
            await eventBus.PublishAsync(new DeleteCategoryEvent(request.categoryId,request.reason,userId)
                ,RoutingKeys.CategoryDeleted,cancellationToken);

            return new DeleteCategoryResponse(
                category.CategoryId,
                category.DeletedAt.Value);
        }
    }
}
