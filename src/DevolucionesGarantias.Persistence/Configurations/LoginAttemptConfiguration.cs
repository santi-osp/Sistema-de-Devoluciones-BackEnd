using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class LoginAttemptConfiguration : IEntityTypeConfiguration<LoginAttempt>
{
    public void Configure(EntityTypeBuilder<LoginAttempt> builder)
    {
        builder.ToTable("login_attempts");

        builder.HasKey(attempt => attempt.Id);

        builder.Property(attempt => attempt.Id).HasColumnName("id");
        builder.Property(attempt => attempt.Correo)
            .HasColumnName("email")
            .HasConversion(email => email.Value, value => new Email(value))
            .HasMaxLength(254)
            .IsRequired();
        builder.Property(attempt => attempt.Succeeded).HasColumnName("succeeded").IsRequired();
        builder.Property(attempt => attempt.FailureReason).HasColumnName("failure_reason").HasMaxLength(500);
        builder.Property(attempt => attempt.IpAddress).HasColumnName("ip_address").HasMaxLength(80);
        builder.Property(attempt => attempt.UserAgent).HasColumnName("user_agent").HasMaxLength(512);
        builder.Property(attempt => attempt.OccurredAt).HasColumnName("occurred_at").IsRequired();

        builder.HasIndex(attempt => attempt.Correo)
            .HasDatabaseName("ix_login_attempts_email");
    }
}
