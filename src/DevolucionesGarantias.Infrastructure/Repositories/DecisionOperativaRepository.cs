using DevolucionesGarantias.Application.Operation.Interfaces;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Infrastructure.Repositories;

public sealed class DecisionOperativaRepository : IDecisionOperativaRepository
{
    private readonly AppDbContext _dbContext;

    public DecisionOperativaRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddAsync(DecisionOperativa decision, CancellationToken cancellationToken = default) =>
        _dbContext.DecisionesOperativas.AddAsync(decision, cancellationToken).AsTask();

    public async Task<IReadOnlyCollection<DecisionOperativa>> ListByRequestAsync(Guid requestId, CancellationToken cancellationToken = default) =>
        await _dbContext.DecisionesOperativas
            .Where(decision => decision.SolicitudId == requestId)
            .OrderByDescending(decision => decision.DecidedAt)
            .ToArrayAsync(cancellationToken);
}
