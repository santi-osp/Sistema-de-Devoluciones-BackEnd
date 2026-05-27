using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;
using DevolucionesGarantias.Domain.States;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class Solicitud : IEntity, IAuditableEntity
{
    private readonly List<Evidencia> _evidencias = [];
    private readonly List<RequestTimeline> _timeline = [];
    private EstadoSolicitud _estado;

    public Solicitud(
        Guid clienteId,
        Guid pedidoId,
        Guid productoId,
        TipoSolicitud tipo,
        string motivo,
        string descripcion,
        int cantidad,
        PreferenciaSolucion preferenciaSolucion,
        string? createdBy = null)
    {
        if (clienteId == Guid.Empty)
        {
            throw new BusinessRuleException("La solicitud debe asociarse a un cliente.");
        }

        if (pedidoId == Guid.Empty)
        {
            throw new BusinessRuleException("La solicitud debe asociarse a un pedido.");
        }

        if (productoId == Guid.Empty)
        {
            throw new BusinessRuleException("La solicitud debe asociarse a un producto.");
        }

        if (string.IsNullOrWhiteSpace(motivo))
        {
            throw new BusinessRuleException("El motivo de la solicitud es obligatorio.");
        }

        if (cantidad <= 0)
        {
            throw new BusinessRuleException("La cantidad solicitada debe ser mayor que cero.");
        }

        Id = Guid.NewGuid();
        ClienteId = clienteId;
        PedidoId = pedidoId;
        ProductoId = productoId;
        Tipo = tipo;
        Motivo = motivo.Trim();
        Descripcion = descripcion?.Trim() ?? string.Empty;
        Cantidad = cantidad;
        PreferenciaSolucion = preferenciaSolucion;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
        EstadoActual = EstadoSolicitudEnum.Creada;
        _estado = new SolicitudCreada();
        RegistrarEvento("Solicitud creada.", null, EstadoActual, createdBy);
    }

    public Guid Id { get; private set; }
    public Guid ClienteId { get; private set; }
    public Guid PedidoId { get; private set; }
    public Guid ProductoId { get; private set; }
    public TipoSolicitud Tipo { get; private set; }
    public string Motivo { get; private set; }
    public string Descripcion { get; private set; }
    public int Cantidad { get; private set; }
    public PreferenciaSolucion PreferenciaSolucion { get; private set; }
    public EstadoSolicitudEnum EstadoActual { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }
    public IReadOnlyCollection<Evidencia> Evidencias => _evidencias.AsReadOnly();
    public IReadOnlyCollection<RequestTimeline> Timeline => _timeline.AsReadOnly();

    public void AgregarEvidencia(Evidencia evidencia, string? updatedBy = null)
    {
        if (evidencia.SolicitudId != Id)
        {
            throw new BusinessRuleException("La evidencia no pertenece a esta solicitud.");
        }

        _evidencias.Add(evidencia);
        RegistrarEvento("Evidencia agregada.", EstadoActual, EstadoActual, updatedBy);
        MarcarActualizacion(updatedBy);
    }

    public void EnviarARevision(string? updatedBy = null)
    {
        EnsureEstadoSincronizado();
        _estado.EnviarARevision(this, updatedBy);
    }

    public void Aprobar(string motivo, string? updatedBy = null)
    {
        EnsureEstadoSincronizado();
        _estado.Aprobar(this, motivo, updatedBy);
    }

    public void Rechazar(string motivo, string? updatedBy = null)
    {
        EnsureEstadoSincronizado();
        _estado.Rechazar(this, motivo, updatedBy);
    }

    public void SolicitarInformacion(string motivo, string? updatedBy = null)
    {
        EnsureEstadoSincronizado();
        _estado.SolicitarInformacion(this, motivo, updatedBy);
    }

    public void Cerrar(string? updatedBy = null)
    {
        EnsureEstadoSincronizado();
        _estado.Cerrar(this, updatedBy);
    }

    // State: la entidad delega la validacion de transiciones al estado actual.
    public void CambiarEstado(EstadoSolicitud nuevoEstado, string? updatedBy = null, string? reason = null)
    {
        EnsureEstadoSincronizado();

        switch (nuevoEstado.Value)
        {
            case EstadoSolicitudEnum.EnRevision:
                _estado.EnviarARevision(this, updatedBy);
                break;
            case EstadoSolicitudEnum.PendienteInformacion:
                _estado.SolicitarInformacion(this, reason ?? string.Empty, updatedBy);
                break;
            case EstadoSolicitudEnum.Aprobada:
                _estado.Aprobar(this, reason ?? string.Empty, updatedBy);
                break;
            case EstadoSolicitudEnum.Rechazada:
                _estado.Rechazar(this, reason ?? string.Empty, updatedBy);
                break;
            case EstadoSolicitudEnum.Cerrada:
                _estado.Cerrar(this, updatedBy);
                break;
            default:
                throw new InvalidStateTransitionException($"No se permite cambiar a {nuevoEstado.Value} desde {EstadoActual}.");
        }
    }

    internal void AplicarEstado(EstadoSolicitud nuevoEstado, string? updatedBy = null, string? reason = null)
    {
        var estadoAnterior = EstadoActual;
        _estado = nuevoEstado;
        EstadoActual = nuevoEstado.Value;
        RegistrarEvento(reason ?? $"Estado cambiado a {EstadoActual}.", estadoAnterior, EstadoActual, updatedBy);
        MarcarActualizacion(updatedBy);
    }

    private void RegistrarEvento(string evento, EstadoSolicitudEnum? anterior, EstadoSolicitudEnum? nuevo, string? createdBy)
    {
        _timeline.Add(new RequestTimeline(Id, evento, anterior, nuevo, createdBy));
    }

    private void MarcarActualizacion(string? updatedBy)
    {
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = updatedBy;
    }

    private void EnsureEstadoSincronizado()
    {
        if (_estado is null || _estado.Value != EstadoActual)
        {
            _estado = EstadoSolicitudFactory.FromEnum(EstadoActual);
        }
    }
}
