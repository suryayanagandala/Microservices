using Invoices.CoreModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invoices.Infrastructure.Persistence;

public sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(invoice => invoice.InvoiceId)
            .HasName("PK_Invoices_InvoiceId");

        builder.Property(invoice => invoice.InvoiceId)
            .UseIdentityByDefaultColumn();

        builder.Property(invoice => invoice.OrderId)
            .IsRequired();

        builder.Property(invoice => invoice.CustomerName)
            .HasColumnType("character varying(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(invoice => invoice.CustomerEmail)
            .HasColumnType("character varying(256)")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(invoice => invoice.InvoiceDate)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(invoice => invoice.Amount)
            .HasPrecision(14, 2)
            .IsRequired();

        builder.Property(invoice => invoice.Tax)
            .HasPrecision(14, 2);

        builder.Property(invoice => invoice.TotalAmount)
            .HasPrecision(14, 2)
            .IsRequired();

        builder.Property(invoice => invoice.PaymentStatus)
            .HasColumnType("character varying(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(invoice => invoice.OrderStatus)
            .HasColumnType("character varying(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(invoice => invoice.PaymentMethod)
            .HasColumnType("character varying(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(invoice => invoice.IsActive)
            .HasDefaultValue(true)
            .IsRequired();
    }
}
