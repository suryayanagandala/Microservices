using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Migration.CoreModels.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Infrastructure.PostgreConfigurations
{
    public class ProductImagePgConfig : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> entity)
        {
            entity.ToTable("productimage");

            entity.HasKey(x => x.ImageId);

            entity.Property(x => x.ImageId)
                .ValueGeneratedNever().HasColumnName("imageid");

            entity.Property(x => x.CreatedAt)
                .HasColumnType("timestamptz").HasColumnName("createdat");

            entity.Property(x => x.IsActive).HasColumnName("isactive");

            entity.Property(x => x.UpdatedAt).HasColumnType("timestamptz").HasColumnName("updatedat");

            entity.Property(x => x.ImageUrl).HasColumnType("text").HasColumnName("imageurl");

            entity.Property(x => x.ProductId).HasColumnName("productid");

            entity.HasOne(x=>x.Product).WithMany().HasForeignKey(x=>x.ProductId);
        }
    }
}
