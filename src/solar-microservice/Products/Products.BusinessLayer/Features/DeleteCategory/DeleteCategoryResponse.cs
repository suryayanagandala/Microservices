using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.DeleteCategory
{
    public record DeleteCategoryResponse(int id,DateTimeOffset deletedat);
}
