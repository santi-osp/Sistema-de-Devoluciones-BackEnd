using DevolucionesGarantias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(rol => rol.Id);

        builder.Property(rol => rol.Id)
            .HasColumnName("id");

        builder.Property(rol => rol.Tipo)
            .HasColumnName("type")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(rol => rol.Nombre)
            .HasColumnName("name")
            .HasMaxLength(80)
            .IsRequired();

        builder.HasIndex(rol => rol.Nombre)
            .IsUnique()
            .HasDatabaseName("ux_roles_name");
    }
}
