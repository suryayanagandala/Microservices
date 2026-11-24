using Migration.Application.Interfaces;
using Migration.CoreModels.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Busniesslayer.Service
{
    public class ProductImageMigrationSrvice : MigrationBase<ProductImage>
    {
        public ProductImageMigrationSrvice(IPostgresRepository<ProductImage> postgresRepository, ISqlRepository<ProductImage> sqlRepository) : base(postgresRepository, sqlRepository)
        {

        }

        protected override Task TransformAsync(List<ProductImage> productImages)
        {
            foreach (var item in productImages)
            {
                item.CreatedAt = item.CreatedAt.ToUniversalTime();
                item.UpdatedAt = item.UpdatedAt?.ToUniversalTime();
               
            }

            return Task.CompletedTask;
        }

        protected override int BatchSize => 5000;
    }
}
