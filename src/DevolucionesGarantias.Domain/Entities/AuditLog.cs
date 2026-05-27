using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class AuditLog : IEntity
{
    public AuditLog(
        Guid? userId,
        string action,
        string entityName,
        string entityId,
        string? oldValues,
        string? newValues,
        string? ipAddress,
        string? userAgent,
        string traceId)
    {
        if (string.IsNullOrWhiteSpace(action))
        {
            throw new BusinessRuleException("La accion de auditoria es obligatoria.");
        }

        if (string.IsNullOrWhiteSpace(entityName))
        {
            throw new BusinessRuleException("La entidad auditada es obligatoria.");
        }

        if (string.IsNullOrWhiteSpace(entityId))
        {
            throw new BusinessRuleException("El identificador de la entidad auditada es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(traceId))
        {
            throw new BusinessRuleException("El TraceId de auditoria es obligatorio.");
        }

        Id = Guid.NewGuid();
        UserId = userId;
        Action = action.Trim();
        EntityName = entityName.Trim();
        EntityId = entityId.Trim();
        OldValues = string.IsNullOrWhiteSpace(oldValues) ? null : oldValues.Trim();
        NewValues = string.IsNullOrWhiteSpace(newValues) ? null : newValues.Trim();
        IpAddress = string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress.Trim();
        UserAgent = string.IsNullOrWhiteSpace(userAgent) ? null : userAgent.Trim();
        TraceId = traceId.Trim();
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid? UserId { get; private set; }
    public string Action { get; private set; }
    public string EntityName { get; private set; }
    public string EntityId { get; private set; }
    public string? OldValues { get; private set; }
    public string? NewValues { get; private set; }
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public string TraceId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
}
