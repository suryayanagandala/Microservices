using Migration.Application.Interfaces;
using Migration.CoreModels.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Busniesslayer.Service
{
    public class ProductsMigrationsService : MigrationBase<Product>
    {
        public ProductsMigrationsService(IPostgresRepository<Product> postgresRepository, ISqlRepository<Product> sqlRepository) : base(postgresRepository, sqlRepository)
        {
        }

        protected override Task TransformAsync(List<Product> records)
        {
            foreach (var record in records)
            {
                record.CreatedAt=record.CreatedAt.ToUniversalTime();
                record.UpdatedAt = record.UpdatedAt?.ToUniversalTime();
                record.DeletedAt = record.DeletedAt?.ToUniversalTime();
            }
            return Task.CompletedTask;
        }
        protected override int BatchSize => 5000;
    }
}
