using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class CasoAsignado : IEntity
{
    public CasoAsignado(Guid solicitudId, Guid proveedorId, string assignedBy)
    {
        if (solicitudId == Guid.Empty)
        {
            throw new BusinessRuleException("El caso asignado debe asociarse a una solicitud.");
        }

        if (proveedorId == Guid.Empty)
        {
            throw new BusinessRuleException("El caso asignado debe asociarse a un proveedor.");
        }

        if (string.IsNullOrWhiteSpace(assignedBy))
        {
            throw new BusinessRuleException("Debe registrarse quien asigna el caso.");
        }

        Id = Guid.NewGuid();
        SolicitudId = solicitudId;
        ProveedorId = proveedorId;
        AssignedBy = assignedBy.Trim();
        AssignedAt = DateTimeOffset.UtcNow;
        Estado = EstadoAsignacionProveedor.Asignado;
    }

    public Guid Id { get; private set; }
    public Guid SolicitudId { get; private set; }
    public Guid ProveedorId { get; private set; }
    public string AssignedBy { get; private set; }
    public DateTimeOffset AssignedAt { get; private set; }
    public EstadoAsignacionProveedor Estado { get; private set; }

    public void CambiarEstado(EstadoAsignacionProveedor estado)
    {
        Estado = estado;
    }
}

