using DevolucionesGarantias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class ValidacionGarantiaConfiguration : IEntityTypeConfiguration<ValidacionGarantia>
{
    public void Configure(EntityTypeBuilder<ValidacionGarantia> builder)
    {
        builder.ToTable("warranty_validations");

        builder.HasKey(validation => validation.Id);

        builder.Property(validation => validation.Id).HasColumnName("id");
        builder.Property(validation => validation.SolicitudId).HasColumnName("request_id").IsRequired();
        builder.Property(validation => validation.ProductoId).HasColumnName("product_id").IsRequired();
        builder.Property(validation => validation.IsWarrantyValid).HasColumnName("is_warranty_valid").IsRequired();
        builder.Property(validation => validation.Reason).HasColumnName("reason").HasMaxLength(1000);
        builder.Property(validation => validation.ValidatedBy).HasColumnName("validated_by").HasMaxLength(150).IsRequired();
        builder.Property(validation => validation.ValidatedAt).HasColumnName("validated_at").IsRequired();

        builder.HasOne<Solicitud>()
            .WithOne()
            .HasForeignKey<ValidacionGarantia>(validation => validation.SolicitudId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Producto>()
            .WithMany()
            .HasForeignKey(validation => validation.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(validation => validation.SolicitudId)
            .IsUnique()
            .HasDatabaseName("ux_warranty_validations_request_id");

        builder.HasIndex(validation => validation.ProductoId)
            .HasDatabaseName("ix_warranty_validations_product_id");
    }
}
