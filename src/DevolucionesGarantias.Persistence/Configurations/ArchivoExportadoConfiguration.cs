using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class ArchivoExportadoConfiguration : IEntityTypeConfiguration<ArchivoExportado>
{
    public void Configure(EntityTypeBuilder<ArchivoExportado> builder)
    {
        builder.ToTable("exported_files");

        builder.HasKey(file => file.Id);

        builder.Property(file => file.Id).HasColumnName("id");
        builder.Property(file => file.ReporteId).HasColumnName("report_id").IsRequired();
        builder.Property(file => file.Formato).HasColumnName("format").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(file => file.NombreArchivo).HasColumnName("file_name").HasMaxLength(255).IsRequired();
        builder.Property(file => file.ExportedBy).HasColumnName("exported_by").HasMaxLength(150).IsRequired();
        builder.Property(file => file.ExportedAt).HasColumnName("exported_at").IsRequired();

        var filePath = builder.Property(file => file.Archivo)
            .HasColumnName("file")
            .HasColumnType("jsonb")
            .HasConversion(ValueObjectConversionHelpers.JsonConverter<FilePath>())
            .IsRequired();

        filePath.Metadata.SetValueComparer(ValueObjectConversionHelpers.JsonComparer<FilePath>());

        builder.HasIndex(file => file.ReporteId)
            .HasDatabaseName("ix_exported_files_report_id");
    }
}
