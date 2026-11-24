using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.CoreModels.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.CoreModels.Configurations
{
    public class ProductImagesConfiguration : IEntityTypeConfiguration<ProductImages>
    {
        public void Configure(EntityTypeBuilder<ProductImages> entity)
        {
            entity.ToTable("productimage");

            entity.HasKey(x=>x.ImageId);

            entity.Property(x => x.ImageId).UseIdentityAlwaysColumn().HasColumnName("imageid");

            entity.Property(x => x.ProductId).IsRequired().HasColumnName("productid");

            entity.Property(x => x.ImageUrl).HasColumnType("Text").IsRequired().HasColumnName("imageurl");

            entity.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired().HasColumnName("createdat");

            entity.Property(x => x.UpdatedAt).HasColumnName("updatedat");

            entity.Property(x => x.IsActive).HasDefaultValue(true).IsRequired().HasColumnName("isactive");

            entity.HasOne(x => x.Product).WithMany(x => x.ProductImages).HasForeignKey(x => x.ProductId)
                .HasConstraintName("fk_productId").OnDelete(DeleteBehavior.Cascade);
        }
    }
}
