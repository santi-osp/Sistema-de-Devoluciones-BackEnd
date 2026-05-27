using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class SolicitudInformacionAdicional : IEntity
{
    public SolicitudInformacionAdicional(Guid solicitudId, string mensaje, DateTimeOffset deadline, string requestedBy)
    {
        if (solicitudId == Guid.Empty)
        {
            throw new BusinessRuleException("La solicitud de informacion debe asociarse a una solicitud.");
        }

        if (string.IsNullOrWhiteSpace(mensaje))
        {
            throw new BusinessRuleException("El mensaje de informacion adicional es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(requestedBy))
        {
            throw new BusinessRuleException("Debe registrarse quien solicita la informacion.");
        }

        Id = Guid.NewGuid();
        SolicitudId = solicitudId;
        Mensaje = mensaje.Trim();
        Deadline = deadline;
        RequestedBy = requestedBy.Trim();
        RequestedAt = DateTimeOffset.UtcNow;
        IsResolved = false;
    }

    public Guid Id { get; private set; }
    public Guid SolicitudId { get; private set; }
    public string Mensaje { get; private set; }
    public DateTimeOffset Deadline { get; private set; }
    public string RequestedBy { get; private set; }
    public DateTimeOffset RequestedAt { get; private set; }
    public bool IsResolved { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }
    public string? Response { get; private set; }

    public void Resolver(string response)
    {
        if (string.IsNullOrWhiteSpace(response))
        {
            throw new BusinessRuleException("La respuesta de informacion adicional es obligatoria.");
        }

        IsResolved = true;
        ResolvedAt = DateTimeOffset.UtcNow;
        Response = response.Trim();
    }
}
