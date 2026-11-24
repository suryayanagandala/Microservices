using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.CreateCategory
{
    public sealed record CreateCategoryResponse
    (int categoryId,string categoryName,DateTimeOffset createdAt,string slug
        );
}
