using DevolucionesGarantias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class DictamenTecnicoConfiguration : IEntityTypeConfiguration<DictamenTecnico>
{
    public void Configure(EntityTypeBuilder<DictamenTecnico> builder)
    {
        builder.ToTable("technical_rulings", table =>
        {
            table.HasCheckConstraint(
                "ck_technical_rulings_no_procede_reason",
                "result <> 'NoProcede' OR length(trim(technical_reason)) > 0");
        });

        builder.HasKey(ruling => ruling.Id);

        builder.Property(ruling => ruling.Id).HasColumnName("id");
        builder.Property(ruling => ruling.SolicitudId).HasColumnName("request_id").IsRequired();
        builder.Property(ruling => ruling.Resultado).HasColumnName("result").HasConversion<string>().HasMaxLength(40).IsRequired();
        builder.Property(ruling => ruling.MotivoTecnico).HasColumnName("technical_reason").HasMaxLength(1500).IsRequired();
        builder.Property(ruling => ruling.Observaciones).HasColumnName("observations").HasMaxLength(2000).IsRequired();
        builder.Property(ruling => ruling.IssuedBy).HasColumnName("issued_by").HasMaxLength(150).IsRequired();
        builder.Property(ruling => ruling.IssuedAt).HasColumnName("issued_at").IsRequired();

        builder.HasOne<Solicitud>()
            .WithOne()
            .HasForeignKey<DictamenTecnico>(ruling => ruling.SolicitudId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ruling => ruling.SolicitudId)
            .IsUnique()
            .HasDatabaseName("ux_technical_rulings_request_id");
    }
}
