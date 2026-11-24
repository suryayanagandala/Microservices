using Microsoft.EntityFrameworkCore;
using Migration.CoreModels.Entites;
using Migration.Infrastructure.PostgreConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Infrastructure.DbSetUp
{
    public class PostgresDbContext : DbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> ProductImages
        {
            get; set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {

                entity.ToTable("categories");

                entity.HasKey(x => x.CategoryId);
                entity.Property(x => x.CategoryId).UseIdentityAlwaysColumn().HasColumnName("categoryid");

                entity.Property(x => x.CategoryName).HasMaxLength(100).IsRequired().HasColumnName("categoryname");

                entity.Property(x => x.Slug).HasMaxLength(200).IsRequired().HasColumnName("slug");


                entity.Property(x => x.IsActive).HasDefaultValue(true).IsRequired().HasColumnName("isactive");

                entity.Property(x => x.CreatedAt).HasColumnName("createdat");

                entity.Property(x => x.Description).HasColumnType("text").IsRequired().HasColumnName("description");
                entity.Property(x => x.UpdatedAt).HasColumnName("updatedat");

                entity.Property(x => x.DeletedReason).HasMaxLength(200).HasColumnName("deletedreason");
                entity.Property(x => x.DeletedAt).HasColumnName("deletedat");
                entity.Property(x => x.ImageUrl).HasColumnName("text").HasColumnName("imageurl");

                entity.Property(x => x.ParentCategoryId).HasColumnName("parentcategoryid");

                entity.HasOne<Category>()
                    .WithMany()
                    .HasForeignKey(e => e.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.ApplyConfiguration(new ProductPgConfig());
            modelBuilder.ApplyConfiguration(new ProductImagePgConfig());
        }
    }
}
