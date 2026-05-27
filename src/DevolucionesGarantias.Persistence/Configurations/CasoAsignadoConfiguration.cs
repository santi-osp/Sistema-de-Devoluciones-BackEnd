using DevolucionesGarantias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class CasoAsignadoConfiguration : IEntityTypeConfiguration<CasoAsignado>
{
    public void Configure(EntityTypeBuilder<CasoAsignado> builder)
    {
        builder.ToTable("assigned_cases");

        builder.HasKey(caseAssigned => caseAssigned.Id);

        builder.Property(caseAssigned => caseAssigned.Id).HasColumnName("id");
        builder.Property(caseAssigned => caseAssigned.SolicitudId).HasColumnName("request_id").IsRequired();
        builder.Property(caseAssigned => caseAssigned.ProveedorId).HasColumnName("provider_id").IsRequired();
        builder.Property(caseAssigned => caseAssigned.AssignedBy).HasColumnName("assigned_by").HasMaxLength(150).IsRequired();
        builder.Property(caseAssigned => caseAssigned.AssignedAt).HasColumnName("assigned_at").IsRequired();
        builder.Property(caseAssigned => caseAssigned.Estado).HasColumnName("status").HasConversion<string>().HasMaxLength(40).IsRequired();

        builder.HasOne<Solicitud>()
            .WithMany()
            .HasForeignKey(caseAssigned => caseAssigned.SolicitudId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Proveedor>()
            .WithMany()
            .HasForeignKey(caseAssigned => caseAssigned.ProveedorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(caseAssigned => caseAssigned.ProveedorId)
            .HasDatabaseName("ix_assigned_cases_provider_id");

        builder.HasIndex(caseAssigned => caseAssigned.SolicitudId)
            .HasDatabaseName("ix_assigned_cases_request_id");
    }
}
