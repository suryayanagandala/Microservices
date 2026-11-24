using Microsoft.EntityFrameworkCore;
using Migration.Application.Utlity;
using Migration.Infrastructure.DbSetUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Application.Service
{
    public   class ProductMigrationService(SqlServerDbContext _sql,PostgresDbContext _pg)
    {
        public  async Task MigrateProduct()
        {
            var data = await _sql.Products.AsNoTracking().ToListAsync();

            foreach (var item in data)
            {
                item.CreatedAt = item.CreatedAt.ToUniversalTime();
                item.UpdatedAt=item.UpdatedAt?.ToUniversalTime();
                item.DeletedAt=item.DeletedAt?.ToUniversalTime();
            }

            BatchClass batch = new BatchClass(_pg);
            await batch.BatchInsert(_pg.Products, data);
        }


        public async Task MigrateProductImage()
        {
            var data=await _sql.ProductImages.AsNoTracking().ToListAsync();

            foreach (var item in data)
            {
                item.CreatedAt=item.CreatedAt.ToUniversalTime();
                item.UpdatedAt = item.UpdatedAt?.ToUniversalTime();
            }

           BatchClass batch=new BatchClass(_pg);
            await batch.BatchInsert(_pg.ProductImages, data);
            
        }
    }
}
