using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.CoreModels.Entities;

namespace Orders.Infrastructure.Persistence;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(item => item.OrderItemId)
            .HasName("PK_OrderItems_OrderItemId");

        builder.Property(item => item.OrderItemId)
            .UseIdentityColumn();

        builder.Property(item => item.ProductId)
            .IsRequired();

        builder.Property(item => item.Quantity)
            .HasDefaultValue(1)
            .IsRequired();

        builder.Property(item => item.UnitPrice)
            .HasPrecision(14, 2)
            .IsRequired();

        builder.Property(item => item.TotalPrice)
            .HasPrecision(14, 2)
            .HasComputedColumnSql("\"Quantity\" * \"UnitPrice\"", stored: true);
    }
}
