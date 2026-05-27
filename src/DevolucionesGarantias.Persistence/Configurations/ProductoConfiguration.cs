using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("products", table =>
        {
            table.HasCheckConstraint("ck_products_quantity_positive", "quantity > 0");
        });

        builder.HasKey(producto => producto.Id);

        builder.Property(producto => producto.Id).HasColumnName("id");
        builder.Property(producto => producto.PedidoId).HasColumnName("order_id").IsRequired();
        builder.Property(producto => producto.Sku).HasColumnName("sku").HasMaxLength(80).IsRequired();
        builder.Property(producto => producto.Nombre).HasColumnName("name").HasMaxLength(180).IsRequired();
        builder.Property(producto => producto.Cantidad).HasColumnName("quantity").IsRequired();
        builder.Property(producto => producto.FechaCompra).HasColumnName("purchase_date").IsRequired();
        builder.Property(producto => producto.GarantiaHasta).HasColumnName("warranty_until").IsRequired();

        var unitPrice = builder.Property(producto => producto.PrecioUnitario)
            .HasColumnName("unit_price")
            .HasColumnType("jsonb")
            .HasConversion(ValueObjectConversionHelpers.JsonConverter<Money>())
            .IsRequired();

        unitPrice.Metadata.SetValueComparer(ValueObjectConversionHelpers.JsonComparer<Money>());

        builder.HasIndex(producto => producto.PedidoId)
            .HasDatabaseName("ix_products_order_id");

        builder.HasIndex(producto => producto.Sku)
            .HasDatabaseName("ix_products_sku");
    }
}
