using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Infrastructure.Repositories;

public sealed class AuditLogRepository
{
    private readonly AppDbContext _dbContext;

    public AuditLogRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default) =>
        _dbContext.AuditLogs.AddAsync(auditLog, cancellationToken).AsTask();

    public async Task<IReadOnlyCollection<AuditLog>> ListByEntityAsync(string entityName, string entityId, CancellationToken cancellationToken = default) =>
        await _dbContext.AuditLogs
            .AsNoTracking()
            .Where(log => log.EntityName == entityName && log.EntityId == entityId)
            .OrderByDescending(log => log.CreatedAt)
            .ToArrayAsync(cancellationToken);
}
