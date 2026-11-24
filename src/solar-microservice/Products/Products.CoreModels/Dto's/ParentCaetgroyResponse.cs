using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.CoreModels.Dto_s
{
    public sealed record ParentCategoryResponse(
    int CategoryId,
    string CategoryName,
    string Slug,
    string? ImageUrl);
}
