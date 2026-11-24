using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Minio.Exceptions;
using Products.ApplicationLayer.Interfaces;
using Products.CoreModels.Dto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.GetParentCategories
{
    public class GetParentCategoriesCommandHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetParentCategoriesRequest, IReadOnlyList<ParentCategoryResponse>>
    {
        public async Task<IReadOnlyList<ParentCategoryResponse>> Handle(GetParentCategoriesRequest request, CancellationToken cancellationToken)
        {
            var categoriesData = await unitOfWork.CategoryInterface.GetParentCategories(cancellationToken);

            if (!categoriesData.Any())
            {
                throw new NotFoundException("No Parent categories found");
            }

            return categoriesData;
        }
    }
}
