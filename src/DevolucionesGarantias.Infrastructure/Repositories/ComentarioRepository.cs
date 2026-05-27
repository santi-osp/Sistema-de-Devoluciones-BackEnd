using DevolucionesGarantias.Application.Operation.Interfaces;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Infrastructure.Repositories;

public sealed class ComentarioRepository : IComentarioRepository
{
    private readonly AppDbContext _dbContext;

    public ComentarioRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<ComentarioInterno>> ListByRequestAsync(Guid requestId, CancellationToken cancellationToken = default) =>
        await _dbContext.ComentariosInternos
            .Where(comment => comment.SolicitudId == requestId)
            .OrderByDescending(comment => comment.CreatedAt)
            .ToArrayAsync(cancellationToken);

    public Task AddAsync(ComentarioInterno comment, CancellationToken cancellationToken = default) =>
        _dbContext.ComentariosInternos.AddAsync(comment, cancellationToken).AsTask();
}
