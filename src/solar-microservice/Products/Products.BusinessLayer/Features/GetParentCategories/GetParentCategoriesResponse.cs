using Products.CoreModels.Dto_s;
using Products.CoreModels.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.GetParentCategories
{
    public record GetParentCategoriesResponse(IReadOnlyList<ParentCategoryResponse> Categories);
    
}
