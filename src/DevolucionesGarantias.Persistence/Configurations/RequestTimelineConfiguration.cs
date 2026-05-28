using DevolucionesGarantias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class RequestTimelineConfiguration : IEntityTypeConfiguration<RequestTimeline>
{
    public void Configure(EntityTypeBuilder<RequestTimeline> builder)
    {
        builder.ToTable("request_timeline");

        builder.HasKey(timeline => timeline.Id);

        builder.Property(timeline => timeline.Id).HasColumnName("id").ValueGeneratedNever();
        builder.Property(timeline => timeline.SolicitudId).HasColumnName("request_id").IsRequired();
        builder.Property(timeline => timeline.Evento).HasColumnName("event").HasMaxLength(600).IsRequired();
        builder.Property(timeline => timeline.EstadoAnterior).HasColumnName("previous_status").HasConversion<string>().HasMaxLength(40);
        builder.Property(timeline => timeline.EstadoNuevo).HasColumnName("new_status").HasConversion<string>().HasMaxLength(40);
        builder.Property(timeline => timeline.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(timeline => timeline.CreatedBy).HasColumnName("created_by").HasMaxLength(150);

        builder.HasIndex(timeline => timeline.SolicitudId)
            .HasDatabaseName("ix_request_timeline_request_id");
    }
}
