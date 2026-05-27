using DevolucionesGarantias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class IndicadorMetricaConfiguration : IEntityTypeConfiguration<IndicadorMetrica>
{
    public void Configure(EntityTypeBuilder<IndicadorMetrica> builder)
    {
        builder.ToTable("metric_indicators");

        builder.HasKey(metric => metric.Id);

        builder.Property(metric => metric.Id).HasColumnName("id");
        builder.Property(metric => metric.ReporteId).HasColumnName("report_id").IsRequired();
        builder.Property(metric => metric.Nombre).HasColumnName("name").HasMaxLength(150).IsRequired();
        builder.Property(metric => metric.Valor).HasColumnName("value").HasPrecision(18, 2).IsRequired();
        builder.Property(metric => metric.Unidad).HasColumnName("unit").HasMaxLength(50).IsRequired();

        builder.HasIndex(metric => metric.ReporteId)
            .HasDatabaseName("ix_metric_indicators_report_id");
    }
}
