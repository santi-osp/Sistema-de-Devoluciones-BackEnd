using DevolucionesGarantias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class SolicitudInformacionAdicionalConfiguration : IEntityTypeConfiguration<SolicitudInformacionAdicional>
{
    public void Configure(EntityTypeBuilder<SolicitudInformacionAdicional> builder)
    {
        builder.ToTable("additional_information_requests");

        builder.HasKey(request => request.Id);

        builder.Property(request => request.Id).HasColumnName("id");
        builder.Property(request => request.SolicitudId).HasColumnName("request_id").IsRequired();
        builder.Property(request => request.Mensaje).HasColumnName("message").HasMaxLength(1500).IsRequired();
        builder.Property(request => request.Deadline).HasColumnName("deadline").IsRequired();
        builder.Property(request => request.RequestedBy).HasColumnName("requested_by").HasMaxLength(150).IsRequired();
        builder.Property(request => request.RequestedAt).HasColumnName("requested_at").IsRequired();
        builder.Property(request => request.IsResolved).HasColumnName("is_resolved").HasDefaultValue(false).IsRequired();
        builder.Property(request => request.ResolvedAt).HasColumnName("resolved_at");
        builder.Property(request => request.Response).HasColumnName("response").HasMaxLength(2000);

        builder.HasOne<Solicitud>()
            .WithMany()
            .HasForeignKey(request => request.SolicitudId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(request => request.SolicitudId)
            .HasDatabaseName("ix_additional_information_requests_request_id");
    }
}
