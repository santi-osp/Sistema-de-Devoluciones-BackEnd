using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class RecepcionProductoConfiguration : IEntityTypeConfiguration<RecepcionProducto>
{
    public void Configure(EntityTypeBuilder<RecepcionProducto> builder)
    {
        builder.ToTable("product_receptions");

        builder.HasKey(reception => reception.Id);

        builder.Property(reception => reception.Id).HasColumnName("id");
        builder.Property(reception => reception.SolicitudId).HasColumnName("request_id").IsRequired();
        builder.Property(reception => reception.Direccion).HasColumnName("address").HasMaxLength(500).IsRequired();
        builder.Property(reception => reception.ReceivedAt).HasColumnName("received_at").IsRequired();
        builder.Property(reception => reception.ReceivedBy).HasColumnName("received_by").HasMaxLength(150).IsRequired();

        var shippingCost = builder.Property(reception => reception.CostoEnvio)
            .HasColumnName("shipping_cost")
            .HasColumnType("jsonb")
            .HasConversion(ValueObjectConversionHelpers.JsonConverter<Money>())
            .IsRequired();

        shippingCost.Metadata.SetValueComparer(ValueObjectConversionHelpers.JsonComparer<Money>());

        builder.HasOne<Solicitud>()
            .WithOne()
            .HasForeignKey<RecepcionProducto>(reception => reception.SolicitudId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(reception => reception.SolicitudId)
            .IsUnique()
            .HasDatabaseName("ux_product_receptions_request_id");
    }
}
