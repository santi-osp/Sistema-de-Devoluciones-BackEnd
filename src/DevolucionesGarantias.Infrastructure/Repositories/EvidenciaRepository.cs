using DevolucionesGarantias.Application.Requests.Interfaces;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Infrastructure.Repositories;

public sealed class EvidenciaRepository : IEvidenciaRepository
{
    private readonly AppDbContext _dbContext;

    public EvidenciaRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Evidencia>> ListByRequestAsync(Guid requestId, CancellationToken cancellationToken = default) =>
        await _dbContext.Evidencias
            .Where(evidence => evidence.SolicitudId == requestId)
            .OrderByDescending(evidence => evidence.UploadedAt)
            .ToArrayAsync(cancellationToken);

    public Task AddAsync(Evidencia evidencia, CancellationToken cancellationToken = default) =>
        _dbContext.Evidencias.AddAsync(evidencia, cancellationToken).AsTask();
}
