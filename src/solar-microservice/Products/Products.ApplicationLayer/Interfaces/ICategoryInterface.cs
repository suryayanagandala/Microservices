using Products.CoreModels.Dto_s;
using Products.CoreModels.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.ApplicationLayer.Interfaces
{
    public interface ICategoryInterface
    {
        Task<bool> ExistCategory(string categoryName,CancellationToken cancellationToken);

        Task<Categories?> GetCategoryById(int categotyId,CancellationToken cancellationToken);

        Task AddCategory(Categories category,CancellationToken cancellationToken);
        Task<bool> ExistsCategory(string categoryName,int excludedCategoryId,CancellationToken cancellationToken);
        Task<Categories?> CheckCategoryForRestore(int categoryId, CancellationToken cancellationToken);
        Task<IReadOnlyList<ParentCategoryResponse>> GetParentCategories(CancellationToken cancellationToken);
    }
}
