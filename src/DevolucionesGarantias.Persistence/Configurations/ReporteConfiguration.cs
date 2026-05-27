using DevolucionesGarantias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class ReporteConfiguration : IEntityTypeConfiguration<Reporte>
{
    public void Configure(EntityTypeBuilder<Reporte> builder)
    {
        builder.ToTable("reports");

        builder.HasKey(reporte => reporte.Id);

        builder.Property(reporte => reporte.Id).HasColumnName("id");
        builder.Property(reporte => reporte.Titulo).HasColumnName("title").HasMaxLength(180).IsRequired();
        var filter = builder.Property(reporte => reporte.Filtro)
            .HasColumnName("filter_json")
            .HasColumnType("jsonb")
            .HasConversion(ValueObjectConversionHelpers.JsonConverter<FiltroReporte>())
            .IsRequired();

        filter.Metadata.SetValueComparer(ValueObjectConversionHelpers.JsonComparer<FiltroReporte>());
        builder.Property(reporte => reporte.GeneratedBy).HasColumnName("generated_by_user_id").HasMaxLength(150).IsRequired();
        builder.Property(reporte => reporte.GeneratedAt).HasColumnName("generated_at").IsRequired();

        builder.HasMany(reporte => reporte.Metricas)
            .WithOne()
            .HasForeignKey(metric => metric.ReporteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(reporte => reporte.Archivos)
            .WithOne()
            .HasForeignKey(file => file.ReporteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(reporte => reporte.GeneratedBy)
            .HasDatabaseName("ix_reports_generated_by_user_id");

        builder.HasIndex(reporte => reporte.GeneratedAt)
            .HasDatabaseName("ix_reports_generated_at");

        builder.Navigation(reporte => reporte.Metricas)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(reporte => reporte.Archivos)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
