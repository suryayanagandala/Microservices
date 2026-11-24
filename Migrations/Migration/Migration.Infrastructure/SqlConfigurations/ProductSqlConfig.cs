using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Migration.CoreModels.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Infrastructure.SqlConfigurations
{
    public class ProductSqlConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.ToTable("Products");

            entity.HasKey(x => x.ProductId);

            entity.Property(x => x.ProductId)
                .UseIdentityColumn(1000, 1);

            entity.Property(x => x.Price)
                .HasColumnType("decimal(15,2)");

            entity.Property(x => x.CreatedAt)
                .HasColumnType("datetimeoffset");
        }
    }
}
