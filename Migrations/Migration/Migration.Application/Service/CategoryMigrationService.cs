using Microsoft.EntityFrameworkCore;
using Migration.CoreModels.Entites;
using Migration.Infrastructure.DbSetUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Application.Service
{
    public class CategoryMigrationService
    {
        private readonly SqlServerDbContext _sql;
        private readonly PostgresDbContext _pg;

        public CategoryMigrationService(
            SqlServerDbContext sql,
            PostgresDbContext pg)
        {
            _sql = sql;
            _pg = pg;
        }

        public async Task MigrateAsync()
        {
            // 1. Read from SQL Server (SOURCE)
            var categories = await _sql.Categories
                .AsNoTracking()
                .OrderBy(x => x.ParentCategoryId == null ? 0 : 1)
                .ThenBy(x => x.CategoryId)
                .ToListAsync();

            if (!categories.Any())
                return;

            // 2. Convert DateTimeOffset  UTC DateTime BEFORE INSERT
            var mappedCategories = categories.Select(x => new Category
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                Slug = x.Slug,
                ParentCategoryId = x.ParentCategoryId,
                IsActive = x.IsActive,
                Description = x.Description,
                ImageUrl = x.ImageUrl,

                //IMPORTANT CONVERSION
                CreatedAt = ToUtc(x.CreatedAt),
                UpdatedAt = x.UpdatedAt.HasValue ? ToUtc(x.UpdatedAt.Value) : null,
                DeletedAt = x.DeletedAt.HasValue ? ToUtc(x.DeletedAt.Value) : null
            }).ToList();

            // 3. Insert into PostgreSQL
            await _pg.Categories.AddRangeAsync(mappedCategories);
            await _pg.SaveChangesAsync();
        }

       
        private DateTime ToUtc(DateTimeOffset dto)
        {
            return dto.UtcDateTime;
        }
    }
}
