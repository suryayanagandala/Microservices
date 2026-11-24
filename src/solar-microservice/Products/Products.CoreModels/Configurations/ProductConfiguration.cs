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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.ToTable("products");
            entity.HasKey(x => x.ProductId);

            entity.Property(x=>x.ProductId).UseIdentityAlwaysColumn().HasIdentityOptions(startValue:1000).HasColumnName("productid");

            entity.Property(x=>x.ProductName).HasMaxLength(200).IsRequired().HasColumnName("productname");

            entity.Property(x => x.CategoryId).IsRequired().HasColumnName("categoryid");

            entity.Property(e => e.Brand).HasMaxLength(200).IsRequired().HasColumnName("brand");

            entity.Property(e => e.Type).HasMaxLength(100).IsRequired().HasColumnName("type");

            entity.Property(e => e.CapacityValue).IsRequired().HasColumnName("capacityvalue");

            entity.Property(e => e.Capacity).HasMaxLength(10).IsRequired().HasColumnName("capacity");

            entity.Property(x => x.Price).HasPrecision(10, 2).IsRequired().HasColumnName("price");

            entity.Property(x => x.WarrantyYears).HasPrecision(5, 2).IsRequired().HasColumnName("warrantyyears");

            entity.Property(x => x.Stock).HasDefaultValue(0).IsRequired().HasColumnName("stock");

            entity.Property(x => x.Description).HasColumnType("text").IsRequired().HasColumnName("description");

            entity.Property(x => x.CreatedAt).HasDefaultValue(DateTimeOffset.Now).IsRequired().HasColumnName("createdat");

            entity.Property(e => e.IsActive).HasDefaultValue(true).IsRequired().HasColumnName("isactive");

            entity.Property(x => x.UpdatedAt).HasColumnName("updatedat");

            entity.Property(x => x.Unit).HasColumnName("unit");

            entity.HasIndex(x=>x.ProductName).IsUnique();

            entity.Property(x => x.DeletedReason).HasMaxLength(100).HasColumnName("deletedreason");

            entity.Property(x => x.DeletedAt).HasColumnName("deletedat");

            entity.HasOne(x=>x.Category).WithMany(x=>x.Products).HasForeignKey(x=>x.CategoryId);

            entity.HasCheckConstraint("chk_capacity", "CapacityValue>0");
            entity.HasCheckConstraint("chk_price", "price > 0");
            entity.HasCheckConstraint("chk_decsription", "length(description) > 10");

        }
    }
}
