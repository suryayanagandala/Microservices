using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.DeleteCategory
{
    public record DeleteCategoryRequest(int categoryId,string reason):ICommand<DeleteCategoryResponse>;

}
