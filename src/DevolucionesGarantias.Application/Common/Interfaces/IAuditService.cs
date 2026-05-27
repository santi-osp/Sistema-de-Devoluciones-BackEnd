namespace DevolucionesGarantias.Application.Common.Interfaces;

public interface IAuditService
{
    Task RegisterAsync(
        Guid? userId,
        string action,
        string entityName,
        string entityId,
        string? oldValues = null,
        string? newValues = null,
        CancellationToken cancellationToken = default);
}
