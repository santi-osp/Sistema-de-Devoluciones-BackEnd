using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class DecisionOperativa : IEntity
{
    public DecisionOperativa(Guid solicitudId, bool approved, string motivo, string decidedBy)
    {
        if (solicitudId == Guid.Empty)
        {
            throw new BusinessRuleException("La decision debe asociarse a una solicitud.");
        }

        if (string.IsNullOrWhiteSpace(motivo))
        {
            throw new BusinessRuleException("Toda decision operativa debe tener un motivo.");
        }

        if (string.IsNullOrWhiteSpace(decidedBy))
        {
            throw new BusinessRuleException("La decision debe registrar quien la tomo.");
        }

        Id = Guid.NewGuid();
        SolicitudId = solicitudId;
        Approved = approved;
        Motivo = motivo.Trim();
        DecidedBy = decidedBy.Trim();
        DecidedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid SolicitudId { get; private set; }
    public bool Approved { get; private set; }
    public string Motivo { get; private set; }
    public string DecidedBy { get; private set; }
    public DateTimeOffset DecidedAt { get; private set; }
}

