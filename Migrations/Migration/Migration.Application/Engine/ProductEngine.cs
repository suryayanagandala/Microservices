using Migration.Application.Service;
using Migration.Infrastructure.DbSetUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Application.Engine
{
    public class ProductEngine
    {
        private readonly PostgresDbContext _pg;

        private readonly SqlServerDbContext _sql;

        public ProductEngine(PostgresDbContext pg,SqlServerDbContext sql)
        {
            _pg = pg;
            _sql = sql;
        }

        public async Task RunMigrations()
        {
            ProductMigrationService service=new ProductMigrationService(_sql,_pg);

            await service.MigrateProduct();
            //await service.MigrateProductImage();
        }
    }
}
