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
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");

            builder.HasKey(x => x.CartItemId);

            builder.Property(x => x.CartItemId).UseIdentityAlwaysColumn().HasIdentityOptions(startValue: 1);

            builder.Property(x => x.ProductId)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .HasDefaultValue(1)
                .IsRequired();

            builder.Property(x => x.UnitPrice)
                .HasPrecision(12, 2)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.UpdatedAt);

            builder.HasIndex(x => new
            {
                x.CartId,
                x.ProductId
            }).IsUnique();
        }
    }
}
