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
    public class ProductImageSqlConfig : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> entity)
        {
            entity.ToTable("ProductImage");

            entity.HasKey(x => x.ImageId);

            entity.Property(x => x.CreatedAt)
                .HasColumnType("datetimeoffset");
        }
    }
}
