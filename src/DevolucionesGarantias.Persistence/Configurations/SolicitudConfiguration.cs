using DevolucionesGarantias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class SolicitudConfiguration : IEntityTypeConfiguration<Solicitud>
{
    public void Configure(EntityTypeBuilder<Solicitud> builder)
    {
        builder.ToTable("requests", table =>
        {
            table.HasCheckConstraint("ck_requests_quantity_positive", "quantity > 0");
            table.HasCheckConstraint("ck_requests_reason_not_empty", "length(trim(reason)) > 0");
        });

        builder.HasKey(solicitud => solicitud.Id);

        builder.Property(solicitud => solicitud.Id).HasColumnName("id");
        builder.Property(solicitud => solicitud.ClienteId).HasColumnName("customer_id").IsRequired();
        builder.Property(solicitud => solicitud.PedidoId).HasColumnName("order_id").IsRequired();
        builder.Property(solicitud => solicitud.ProductoId).HasColumnName("product_id").IsRequired();
        builder.Property(solicitud => solicitud.Tipo).HasColumnName("type").HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(solicitud => solicitud.Motivo).HasColumnName("reason").HasMaxLength(500).IsRequired();
        builder.Property(solicitud => solicitud.Descripcion).HasColumnName("description").HasMaxLength(2000).IsRequired();
        builder.Property(solicitud => solicitud.Cantidad).HasColumnName("quantity").IsRequired();
        builder.Property(solicitud => solicitud.PreferenciaSolucion).HasColumnName("solution_preference").HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(solicitud => solicitud.EstadoActual).HasColumnName("status").HasConversion<string>().HasMaxLength(40).IsRequired();
        builder.Property(solicitud => solicitud.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(solicitud => solicitud.CreatedBy).HasColumnName("created_by").HasMaxLength(150);
        builder.Property(solicitud => solicitud.UpdatedAt).HasColumnName("updated_at");
        builder.Property(solicitud => solicitud.UpdatedBy).HasColumnName("updated_by").HasMaxLength(150);

        builder.HasOne<Cliente>()
            .WithMany()
            .HasForeignKey(solicitud => solicitud.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Pedido>()
            .WithMany()
            .HasForeignKey(solicitud => solicitud.PedidoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Producto>()
            .WithMany()
            .HasForeignKey(solicitud => solicitud.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(solicitud => solicitud.Evidencias)
            .WithOne()
            .HasForeignKey(evidencia => evidencia.SolicitudId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(solicitud => solicitud.Timeline)
            .WithOne()
            .HasForeignKey(timeline => timeline.SolicitudId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(solicitud => solicitud.ClienteId)
            .HasDatabaseName("ix_requests_customer_id");

        builder.HasIndex(solicitud => solicitud.PedidoId)
            .HasDatabaseName("ix_requests_order_id");

        builder.HasIndex(solicitud => solicitud.ProductoId)
            .HasDatabaseName("ix_requests_product_id");

        builder.HasIndex(solicitud => solicitud.EstadoActual)
            .HasDatabaseName("ix_requests_status");

        builder.HasIndex(solicitud => solicitud.Tipo)
            .HasDatabaseName("ix_requests_type");

        builder.HasIndex(solicitud => solicitud.CreatedAt)
            .HasDatabaseName("ix_requests_created_at");

        builder.HasIndex(solicitud => new { solicitud.ClienteId, solicitud.PedidoId, solicitud.ProductoId, solicitud.Tipo, solicitud.Motivo })
            .IsUnique()
            .HasDatabaseName("ux_requests_duplicate_guard");

        builder.Navigation(solicitud => solicitud.Evidencias)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(solicitud => solicitud.Timeline)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
