using DevolucionesGarantias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class DecisionOperativaConfiguration : IEntityTypeConfiguration<DecisionOperativa>
{
    public void Configure(EntityTypeBuilder<DecisionOperativa> builder)
    {
        builder.ToTable("operational_decisions", table =>
        {
            table.HasCheckConstraint("ck_operational_decisions_reason_not_empty", "length(trim(reason)) > 0");
        });

        builder.HasKey(decision => decision.Id);

        builder.Property(decision => decision.Id).HasColumnName("id");
        builder.Property(decision => decision.SolicitudId).HasColumnName("request_id").IsRequired();
        builder.Property(decision => decision.Approved).HasColumnName("approved").IsRequired();
        builder.Property(decision => decision.Motivo).HasColumnName("reason").HasMaxLength(1000).IsRequired();
        builder.Property(decision => decision.DecidedBy).HasColumnName("decided_by").HasMaxLength(150).IsRequired();
        builder.Property(decision => decision.DecidedAt).HasColumnName("decided_at").IsRequired();

        builder.HasOne<Solicitud>()
            .WithMany()
            .HasForeignKey(decision => decision.SolicitudId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(decision => decision.SolicitudId)
            .HasDatabaseName("ix_operational_decisions_request_id");
    }
}
