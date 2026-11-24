using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.CoreModels.Entities;

namespace Orders.Infrastructure.Persistence;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", tableBuilder =>
            tableBuilder.HasCheckConstraint(
                "CHK_Orders_Status_New",
                "\"Status\" IN ('CREATED', 'PAYMENT_PENDING', 'PAID', 'COD_PENDING', 'CONFIRMED', 'SHIPPED', 'DELIVERED', 'CANCELLED', 'PAYMENT_FAILED')"));

        builder.HasKey(order => order.OrderId)
            .HasName("PK_Orders_OrderId");

        builder.Property(order => order.OrderId)
            .UseIdentityByDefaultColumn()
            .HasIdentityOptions(startValue: 100, incrementBy: 1);

        builder.Property(order => order.UserId)
            .HasColumnType("character varying(6)")
            .HasMaxLength(6)
            .IsRequired();

        builder.Property(order => order.OrderDate)
            .HasColumnType("date")
            .HasDefaultValueSql("CURRENT_DATE")
            .IsRequired();

        builder.Property(order => order.Status)
            .HasConversion<string>()
            .HasColumnType("character varying(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(order => order.TotalAmount)
            .HasPrecision(14, 2)
            .HasDefaultValue(0.00m)
            .IsRequired();

        builder.Property(order => order.DeliveryAddress)
            .HasColumnType("character varying(1000)")
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(order => order.PhoneNumber)
            .HasColumnType("character varying(10)")
            .HasMaxLength(10);

        builder.Property(order => order.FullName)
            .HasColumnType("character varying(255)")
            .HasMaxLength(255);

        builder.Property(order => order.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(order => order.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(order => order.UpdatedAt)
            .HasColumnType("timestamp with time zone");

        builder.HasMany(order => order.OrderItems)
            .WithOne(item => item.Order)
            .HasForeignKey(item => item.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_OrderItems_OrderId");
    }
}
