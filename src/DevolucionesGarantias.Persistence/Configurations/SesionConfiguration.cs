using DevolucionesGarantias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class SesionConfiguration : IEntityTypeConfiguration<Sesion>
{
    public void Configure(EntityTypeBuilder<Sesion> builder)
    {
        builder.ToTable("sessions");

        builder.HasKey(sesion => sesion.Id);

        builder.Property(sesion => sesion.Id).HasColumnName("id");
        builder.Property(sesion => sesion.UsuarioId).HasColumnName("user_id").IsRequired();
        builder.Property(sesion => sesion.Token).HasColumnName("token").HasMaxLength(1024).IsRequired();
        builder.Property(sesion => sesion.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(sesion => sesion.ExpiresAt).HasColumnName("expires_at").IsRequired();
        builder.Property(sesion => sesion.IsActive).HasColumnName("is_active").HasDefaultValue(true).IsRequired();

        builder.Ignore(sesion => sesion.IsExpired);

        builder.HasIndex(sesion => sesion.UsuarioId)
            .HasDatabaseName("ix_sessions_user_id");
    }
}
