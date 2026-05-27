using DevolucionesGarantias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_logs");

        builder.HasKey(log => log.Id);

        builder.Property(log => log.Id).HasColumnName("id");
        builder.Property(log => log.UserId).HasColumnName("user_id");
        builder.Property(log => log.Action).HasColumnName("action").HasMaxLength(120).IsRequired();
        builder.Property(log => log.EntityName).HasColumnName("entity_name").HasMaxLength(150).IsRequired();
        builder.Property(log => log.EntityId).HasColumnName("entity_id").HasMaxLength(120).IsRequired();
        builder.Property(log => log.OldValues).HasColumnName("old_values").HasColumnType("jsonb");
        builder.Property(log => log.NewValues).HasColumnName("new_values").HasColumnType("jsonb");
        builder.Property(log => log.IpAddress).HasColumnName("ip_address").HasMaxLength(80);
        builder.Property(log => log.UserAgent).HasColumnName("user_agent").HasMaxLength(512);
        builder.Property(log => log.TraceId).HasColumnName("trace_id").HasMaxLength(120).IsRequired();
        builder.Property(log => log.CreatedAt).HasColumnName("created_at").IsRequired();

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(log => log.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(log => log.UserId)
            .HasDatabaseName("ix_audit_logs_user_id");

        builder.HasIndex(log => new { log.EntityName, log.EntityId })
            .HasDatabaseName("ix_audit_logs_entity");

        builder.HasIndex(log => log.CreatedAt)
            .HasDatabaseName("ix_audit_logs_created_at");
    }
}
