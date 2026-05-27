using DevolucionesGarantias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevolucionesGarantias.Persistence.Configurations;

public sealed class ComentarioInternoConfiguration : IEntityTypeConfiguration<ComentarioInterno>
{
    public void Configure(EntityTypeBuilder<ComentarioInterno> builder)
    {
        builder.ToTable("internal_comments");

        builder.HasKey(comment => comment.Id);

        builder.Property(comment => comment.Id).HasColumnName("id");
        builder.Property(comment => comment.SolicitudId).HasColumnName("request_id").IsRequired();
        builder.Property(comment => comment.Texto).HasColumnName("text").HasMaxLength(2000).IsRequired();
        builder.Property(comment => comment.Autor).HasColumnName("author").HasMaxLength(150).IsRequired();
        builder.Property(comment => comment.VisibleToCustomer).HasColumnName("visible_to_customer").HasDefaultValue(false).IsRequired();
        builder.Property(comment => comment.CreatedAt).HasColumnName("created_at").IsRequired();

        builder.HasOne<Solicitud>()
            .WithMany()
            .HasForeignKey(comment => comment.SolicitudId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(comment => comment.SolicitudId)
            .HasDatabaseName("ix_internal_comments_request_id");
    }
}
