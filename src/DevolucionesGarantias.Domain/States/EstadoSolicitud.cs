using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Domain.States;

// State: encapsula las transiciones permitidas para cada estado de una solicitud.
public abstract class EstadoSolicitud
{
    protected EstadoSolicitud(EstadoSolicitudEnum value)
    {
        Value = value;
    }

    public EstadoSolicitudEnum Value { get; }

    public virtual void EnviarARevision(Solicitud solicitud, string? updatedBy = null)
    {
        throw InvalidTransition(nameof(EnviarARevision));
    }

    public virtual void CompletarRevisionProveedor(Solicitud solicitud, string motivo, string? updatedBy = null)
    {
        throw InvalidTransition(nameof(CompletarRevisionProveedor));
    }

    public virtual void Aprobar(Solicitud solicitud, string motivo, string? updatedBy = null)
    {
        throw InvalidTransition(nameof(Aprobar));
    }

    public virtual void Rechazar(Solicitud solicitud, string motivo, string? updatedBy = null)
    {
        throw InvalidTransition(nameof(Rechazar));
    }

    public virtual void SolicitarInformacion(Solicitud solicitud, string motivo, string? updatedBy = null)
    {
        throw InvalidTransition(nameof(SolicitarInformacion));
    }

    public virtual void Cerrar(Solicitud solicitud, string? updatedBy = null)
    {
        throw InvalidTransition(nameof(Cerrar));
    }

    protected static void EnsureMotivo(string motivo)
    {
        if (string.IsNullOrWhiteSpace(motivo))
        {
            throw new BusinessRuleException("El motivo de la transicion es obligatorio.");
        }
    }

    private InvalidStateTransitionException InvalidTransition(string action)
    {
        return new InvalidStateTransitionException($"No se permite ejecutar {action} desde el estado {Value}.");
    }
}

