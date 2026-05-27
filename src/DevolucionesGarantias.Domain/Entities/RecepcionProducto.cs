using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;
using DevolucionesGarantias.Domain.ValueObjects;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class RecepcionProducto : IEntity
{
    public RecepcionProducto(Guid solicitudId, string direccion, Money costoEnvio, DateTimeOffset receivedAt, string receivedBy)
    {
        if (solicitudId == Guid.Empty)
        {
            throw new BusinessRuleException("La recepcion del producto debe asociarse a una solicitud.");
        }

        if (string.IsNullOrWhiteSpace(direccion))
        {
            throw new BusinessRuleException("La direccion de recepcion es obligatoria.");
        }

        if (string.IsNullOrWhiteSpace(receivedBy))
        {
            throw new BusinessRuleException("Debe registrarse quien recibe el producto.");
        }

        Id = Guid.NewGuid();
        SolicitudId = solicitudId;
        Direccion = direccion.Trim();
        CostoEnvio = costoEnvio;
        ReceivedAt = receivedAt;
        ReceivedBy = receivedBy.Trim();
    }

    public Guid Id { get; private set; }
    public Guid SolicitudId { get; private set; }
    public string Direccion { get; private set; }
    public Money CostoEnvio { get; private set; }
    public DateTimeOffset ReceivedAt { get; private set; }
    public string ReceivedBy { get; private set; }
}

