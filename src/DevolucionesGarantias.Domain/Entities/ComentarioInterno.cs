using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class ComentarioInterno : IEntity
{
    public ComentarioInterno(Guid solicitudId, string texto, string autor, bool visibleToCustomer = false)
    {
        if (solicitudId == Guid.Empty)
        {
            throw new BusinessRuleException("El comentario debe asociarse a una solicitud.");
        }

        if (string.IsNullOrWhiteSpace(texto))
        {
            throw new BusinessRuleException("El texto del comentario es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(autor))
        {
            throw new BusinessRuleException("El autor del comentario es obligatorio.");
        }

        Id = Guid.NewGuid();
        SolicitudId = solicitudId;
        Texto = texto.Trim();
        Autor = autor.Trim();
        VisibleToCustomer = visibleToCustomer;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid SolicitudId { get; private set; }
    public string Texto { get; private set; }
    public string Autor { get; private set; }
    public bool VisibleToCustomer { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
}

