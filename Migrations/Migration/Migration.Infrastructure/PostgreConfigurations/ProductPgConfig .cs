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
    public class ProductPgConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.ToTable("products");

            entity.HasKey(x => x.ProductId);

            entity.Property(x => x.ProductId)
                .ValueGeneratedNever().HasColumnName("productid");

            entity.Property(x => x.Price)
                .HasColumnType("numeric(15,2)").HasColumnName("price");

            entity.Property(x => x.CreatedAt)
                .HasColumnType("timestamptz").HasColumnName("createdat");

            entity.Property(x => x.UpdatedAt).HasColumnName("updatedat");

            entity.Property(x => x.Type).HasColumnName("type");

            entity.Property(x=>x.Capacity).HasColumnName("capacity");

            entity.Property(x => x.CapacityValue).HasColumnName("capacityvalue");

            entity.Property(x => x.Stock).HasColumnName("stock");

            entity.Property(x => x.DeletedAt).HasColumnName("deletedat");

            entity.Property(x => x.DeletedReason).HasColumnName("deletedreason");

            entity.Property(x => x.Description).HasColumnName("description");

            entity.Property(x => x.Warranty_Years).HasColumnName("warrantyyears");

            entity.Property(x => x.Brand).HasColumnName("brand");

            entity.Property(x => x.CategoryId).HasColumnName("categoryid");

            entity.Property(x => x.ProductName).HasColumnName("productname");

            entity.Property(x => x.Price).HasColumnName("price");

            entity.Property(x => x.IsActive).HasColumnName("isactive");

            entity.Property(x => x.Unit).HasColumnName("unit");

            entity.HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId);
        }
    }
}
