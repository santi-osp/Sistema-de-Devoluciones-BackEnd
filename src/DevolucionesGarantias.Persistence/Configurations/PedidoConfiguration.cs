using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(pedido => pedido.Id);

        builder.Property(pedido => pedido.Id).HasColumnName("id");
        builder.Property(pedido => pedido.ClienteId).HasColumnName("customer_id").IsRequired();
        builder.Property(pedido => pedido.Numero).HasColumnName("number").HasMaxLength(80).IsRequired();
        builder.Property(pedido => pedido.FechaCompra).HasColumnName("purchase_date").IsRequired();

        var total = builder.Property(pedido => pedido.Total)
            .HasColumnName("total")
            .HasColumnType("jsonb")
            .HasConversion(ValueObjectConversionHelpers.JsonConverter<Money>())
            .IsRequired();

        total.Metadata.SetValueComparer(ValueObjectConversionHelpers.JsonComparer<Money>());

        builder.HasOne<Cliente>()
            .WithMany()
            .HasForeignKey(pedido => pedido.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(pedido => pedido.Productos)
            .WithOne()
            .HasForeignKey(producto => producto.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pedido => pedido.ClienteId)
            .HasDatabaseName("ix_orders_customer_id");

        builder.HasIndex(pedido => pedido.Numero)
            .IsUnique()
            .HasDatabaseName("ux_orders_number");

        builder.Navigation(pedido => pedido.Productos)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
