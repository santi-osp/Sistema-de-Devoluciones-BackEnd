using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class RequestTimeline : IEntity
{
    public RequestTimeline(Guid solicitudId, string evento, EstadoSolicitudEnum? estadoAnterior, EstadoSolicitudEnum? estadoNuevo, string? createdBy = null)
    {
        if (solicitudId == Guid.Empty)
        {
            throw new BusinessRuleException("El evento de historial debe asociarse a una solicitud.");
        }

        if (string.IsNullOrWhiteSpace(evento))
        {
            throw new BusinessRuleException("El evento de historial es obligatorio.");
        }

        Id = Guid.NewGuid();
        SolicitudId = solicitudId;
        Evento = evento.Trim();
        EstadoAnterior = estadoAnterior;
        EstadoNuevo = estadoNuevo;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    public Guid Id { get; private set; }
    public Guid SolicitudId { get; private set; }
    public string Evento { get; private set; }
    public EstadoSolicitudEnum? EstadoAnterior { get; private set; }
    public EstadoSolicitudEnum? EstadoNuevo { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
}

