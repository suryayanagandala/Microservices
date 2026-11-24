using Microsoft.EntityFrameworkCore;
using Products.ApplicationLayer.Interfaces;
using Products.CoreModels;
using Products.CoreModels.Dto_s;
using Products.CoreModels.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.DbAccessLayer.Repositories
{
    public class CategoryRepository(ProductDbContext _db) : ICategoryInterface
    {
        
        public async Task AddCategory(Categories category, CancellationToken cancellationToken)
        {
            await _db.Categories.AddAsync(category,cancellationToken);
        }

        public async Task<Categories?> CheckCategoryForRestore(int categoryId,CancellationToken cancellationToken)
        {
           return  await _db.Categories.FirstOrDefaultAsync(x => x.CategoryId == categoryId && !x.IsActive,cancellationToken);
        }

        public async Task<bool> ExistCategory(string categoryName, CancellationToken cancellationToken)
        {
            return await _db.Categories.AnyAsync(x=>x.CategoryName == categoryName);
        }

        public Task<bool> ExistsCategory(string categoryName, int excludedCategoryId, CancellationToken cancellationToken)
        {
            return _db.Categories.AnyAsync(x =>x.CategoryName == categoryName &&x.CategoryId != excludedCategoryId,
                     cancellationToken);
        }

        public async Task<Categories?> GetCategoryById(int categotyId, CancellationToken cancellationToken)
        {
            return await _db.Categories.Include(x=>x.ChildCategories).Include(x=>x.Products).FirstOrDefaultAsync(x=>x.CategoryId==categotyId&&x.IsActive,cancellationToken);
        }

        public async Task<IReadOnlyList<ParentCategoryResponse>> GetParentCategories(CancellationToken cancellationToken)
        {
            return await _db.Set<Categories>().AsNoTracking().Where(x => x.CategoryParentId == null && x.IsActive)
                .Select(x => new ParentCategoryResponse(x.CategoryId,x.CategoryName,x.Slug,x.ImageUrl
                    )).ToListAsync(cancellationToken);
        }
    }
}
