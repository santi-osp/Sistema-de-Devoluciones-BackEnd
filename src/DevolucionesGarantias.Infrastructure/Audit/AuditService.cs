using System.Text.Json;
using DevolucionesGarantias.Application.Common.Interfaces;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;

namespace DevolucionesGarantias.Infrastructure.Audit;

public sealed class AuditService : IAuditService
{
    private readonly AuditLogRepository _auditLogs;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditService(AuditLogRepository auditLogs, IHttpContextAccessor httpContextAccessor)
    {
        _auditLogs = auditLogs;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task RegisterAsync(
        Guid? userId,
        string action,
        string entityName,
        string entityId,
        string? oldValues = null,
        string? newValues = null,
        CancellationToken cancellationToken = default)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var auditLog = new AuditLog(
            userId,
            action,
            entityName,
            entityId,
            NormalizeJsonValue(oldValues),
            NormalizeJsonValue(newValues),
            httpContext?.Connection.RemoteIpAddress?.ToString(),
            httpContext?.Request.Headers.UserAgent.ToString(),
            httpContext?.TraceIdentifier ?? Guid.NewGuid().ToString("N"));

        await _auditLogs.AddAsync(auditLog, cancellationToken);
    }

    private static string? NormalizeJsonValue(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();

        try
        {
            using var _ = JsonDocument.Parse(trimmed);
            return trimmed;
        }
        catch (JsonException)
        {
            return JsonSerializer.Serialize(trimmed);
        }
    }
}
