using Cart.CoreModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Infrastructure.Persistance
{
    public class CartConfiguration : IEntityTypeConfiguration<Carts>
    {
        public void Configure(EntityTypeBuilder<Carts> builder)
        {
            builder.ToTable("Cart");

            builder.HasKey(x => x.CartId);

            builder.Property(x => x.CartId).UseIdentityAlwaysColumn().HasIdentityOptions(startValue: 1);

            builder.Property(x => x.UserId)
                .HasMaxLength(6)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(x => x.UpdatedAt);

            builder.HasMany(x => x.CartItems)
                .WithOne(x => x.Cart)
                .HasForeignKey(x => x.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
