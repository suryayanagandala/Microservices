using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payments.CoreModels.Entities;

namespace Payments.Infrastructure.Persistence;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint(
                "CHK_Payments_TransactionId",
                "char_length(\"TransactionId\") = 15");

            tableBuilder.HasCheckConstraint(
                "CHK_Payments_Amount",
                "\"Amount\" > 0");
        });

        builder.HasKey(payment => payment.PaymentId)
            .HasName("PK_Payments_PaymentId");

        builder.Property(payment => payment.PaymentId)
            .UseIdentityByDefaultColumn()
            .HasIdentityOptions(startValue: 1000, incrementBy: 1);

        builder.Property(payment => payment.OrderId)
            .IsRequired();

        builder.HasIndex(payment => payment.OrderId)
            .IsUnique()
            .HasDatabaseName("UX_Payments_OrderId");

        builder.Property(payment => payment.PaymentMethod)
            .HasColumnType("character varying(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(payment => payment.PaymentStatus)
            .HasColumnType("character varying(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(payment => payment.TransactionId)
            .HasColumnType("character varying(15)")
            .HasMaxLength(15)
            .IsRequired();

        builder.Property(payment => payment.Amount)
            .HasPrecision(15, 2)
            .IsRequired();

        builder.Property(payment => payment.PaymentDate)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(payment => payment.IsActive)
            .HasDefaultValue(true)
            .IsRequired();
    }
}
