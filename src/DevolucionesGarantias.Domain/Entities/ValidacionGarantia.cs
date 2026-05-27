using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class ValidacionGarantia : IEntity
{
    public ValidacionGarantia(Guid solicitudId, Guid productoId, bool isWarrantyValid, string validatedBy, string? reason = null)
    {
        if (solicitudId == Guid.Empty)
        {
            throw new BusinessRuleException("La validacion de garantia debe asociarse a una solicitud.");
        }

        if (productoId == Guid.Empty)
        {
            throw new BusinessRuleException("La validacion de garantia debe asociarse a un producto.");
        }

        if (string.IsNullOrWhiteSpace(validatedBy))
        {
            throw new BusinessRuleException("Debe registrarse quien valida la garantia.");
        }

        Id = Guid.NewGuid();
        SolicitudId = solicitudId;
        ProductoId = productoId;
        IsWarrantyValid = isWarrantyValid;
        Reason = string.IsNullOrWhiteSpace(reason) ? null : reason.Trim();
        ValidatedBy = validatedBy.Trim();
        ValidatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid SolicitudId { get; private set; }
    public Guid ProductoId { get; private set; }
    public bool IsWarrantyValid { get; private set; }
    public string? Reason { get; private set; }
    public string ValidatedBy { get; private set; }
    public DateTimeOffset ValidatedAt { get; private set; }
}

