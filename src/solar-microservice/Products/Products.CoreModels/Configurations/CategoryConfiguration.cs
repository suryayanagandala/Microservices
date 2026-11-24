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
    public class CategoryConfiguration : IEntityTypeConfiguration<Categories>
    {
        public void Configure(EntityTypeBuilder<Categories> entity)
        {
            entity.ToTable("categories");

            entity.HasKey(x => x.CategoryId);
            entity.Property(x => x.CategoryId).UseIdentityAlwaysColumn().HasColumnName("categoryid");

            entity.Property(x => x.CategoryName).HasMaxLength(100).IsRequired().HasColumnName("categoryname");

            entity.Property(x => x.Slug).HasMaxLength(200).IsRequired().HasColumnName("slug");
     

            entity.Property(x=>x.IsActive).HasDefaultValue(true).IsRequired().HasColumnName("isactive");

            entity.Property(x => x.CreatedAt).HasColumnName("createdat").ValueGeneratedOnAdd();

              entity.Property(x => x.Description).HasColumnType("text").IsRequired().HasColumnName("description");
            entity.Property(x => x.UpdatedAt).HasColumnName("updatedat");

            entity.Property(x => x.DeletedReason).HasMaxLength(200).HasColumnName("deletedreason");
            entity.Property(x => x.DeletedAt).HasColumnName("deletedat");
            entity.Property(x => x.ImageUrl).HasColumnName("text").HasColumnName("imageurl");

            entity.Property(x => x.CategoryParentId).HasColumnName("parentcategoryid");

            entity.HasOne(x => x.ParentCategory).WithMany(x => x.ChildCategories)
                .HasForeignKey(x => x.CategoryParentId)
                .HasConstraintName("fk_CategoryParentId").OnDelete(DeleteBehavior.Restrict);
        }
    }
}
