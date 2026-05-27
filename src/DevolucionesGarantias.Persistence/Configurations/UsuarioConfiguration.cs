using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("users");

        builder.HasKey(usuario => usuario.Id);

        builder.Property(usuario => usuario.Id)
            .HasColumnName("id");

        builder.Property(usuario => usuario.Nombre)
            .HasColumnName("name")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(usuario => usuario.Correo)
            .HasColumnName("email")
            .HasConversion(email => email.Value, value => new Email(value))
            .HasMaxLength(254)
            .IsRequired();

        builder.Property(usuario => usuario.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(usuario => usuario.UltimoAcceso)
            .HasColumnName("last_access_at");

        builder.Property(usuario => usuario.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(usuario => usuario.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(150);

        builder.Property(usuario => usuario.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(usuario => usuario.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(150);

        builder.HasIndex(usuario => usuario.Correo)
            .IsUnique()
            .HasDatabaseName("ux_users_email");

        builder.Property<string>("user_type")
            .HasColumnName("user_type")
            .HasMaxLength(30)
            .IsRequired();

        builder.HasDiscriminator<string>("user_type")
            .HasValue<Cliente>("cliente")
            .HasValue<Administrador>("administrador")
            .HasValue<Proveedor>("proveedor");

        builder.HasMany(usuario => usuario.Roles)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "user_roles",
                right => right.HasOne<Rol>()
                    .WithMany()
                    .HasForeignKey("role_id")
                    .OnDelete(DeleteBehavior.Restrict),
                left => left.HasOne<Usuario>()
                    .WithMany()
                    .HasForeignKey("user_id")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("user_roles");
                    join.HasKey("user_id", "role_id");
                    join.IndexerProperty<Guid>("user_id").HasColumnName("user_id");
                    join.IndexerProperty<Guid>("role_id").HasColumnName("role_id");
                    join.HasIndex("role_id").HasDatabaseName("ix_user_roles_role_id");
                });

        builder.HasMany(usuario => usuario.Sesiones)
            .WithOne()
            .HasForeignKey(sesion => sesion.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(usuario => usuario.Roles)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(usuario => usuario.Sesiones)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
