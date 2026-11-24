using Microsoft.EntityFrameworkCore;
using Migration.CoreModels.Entites;
using Migration.Infrastructure.SqlConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Infrastructure.DbSetUp
{
    public class SqlServerDbContext : DbContext
    {
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }


        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");

                entity.HasKey(e => e.CategoryId);

                entity.Property(x => x.CategoryId).HasColumnName("categoryid");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(100);

                entity.Property(e => e.Slug)
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .HasColumnType("text");

                entity.Property(e => e.ImageUrl)
                    .HasColumnType("text");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);
            });

            modelBuilder.ApplyConfiguration(new ProductSqlConfig());
            modelBuilder.ApplyConfiguration(new ProductImageSqlConfig());
        }
    }
}
