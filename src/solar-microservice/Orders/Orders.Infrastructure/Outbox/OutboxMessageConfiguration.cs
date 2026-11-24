using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Orders.Infrastructure.Outbox;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");
        builder.HasKey(message => message.EventId);

        builder.Property(message => message.EventType)
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(message => message.RoutingKey)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(message => message.Payload)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(message => message.LastError)
            .HasMaxLength(4000);

        builder.HasIndex(message => new
            {
                message.ProcessedAt,
                message.NextAttemptAt,
                message.CreatedAt
            })
            .HasDatabaseName("IX_outbox_messages_pending");
    }
}
