using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class EvidenciaConfiguration : IEntityTypeConfiguration<Evidencia>
{
    public void Configure(EntityTypeBuilder<Evidencia> builder)
    {
        builder.ToTable("evidence", table =>
        {
            table.HasCheckConstraint("ck_evidence_size_non_negative", "size_in_bytes >= 0");
        });

        builder.HasKey(evidencia => evidencia.Id);

        builder.Property(evidencia => evidencia.Id).HasColumnName("id");
        builder.Property(evidencia => evidencia.SolicitudId).HasColumnName("request_id").IsRequired();
        builder.Property(evidencia => evidencia.Tipo).HasColumnName("type").HasConversion<string>().HasMaxLength(40).IsRequired();
        builder.Property(evidencia => evidencia.NombreArchivo).HasColumnName("file_name").HasMaxLength(255).IsRequired();
        builder.Property(evidencia => evidencia.SizeInBytes).HasColumnName("size_in_bytes").IsRequired();
        builder.Property(evidencia => evidencia.UploadedBy).HasColumnName("uploaded_by").HasMaxLength(150);
        builder.Property(evidencia => evidencia.UploadedAt).HasColumnName("uploaded_at").IsRequired();

        var filePath = builder.Property(evidencia => evidencia.Archivo)
            .HasColumnName("file")
            .HasColumnType("jsonb")
            .HasConversion(ValueObjectConversionHelpers.JsonConverter<FilePath>())
            .IsRequired();

        filePath.Metadata.SetValueComparer(ValueObjectConversionHelpers.JsonComparer<FilePath>());

        builder.HasIndex(evidencia => evidencia.SolicitudId)
            .HasDatabaseName("ix_evidence_request_id");
    }
}
