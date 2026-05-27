using DevolucionesGarantias.Application.Providers.Interfaces;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Infrastructure.Repositories;

public sealed class DictamenRepository : IDictamenRepository
{
    private readonly AppDbContext _dbContext;

    public DictamenRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddAsync(DictamenTecnico dictamen, CancellationToken cancellationToken = default) =>
        _dbContext.DictamenesTecnicos.AddAsync(dictamen, cancellationToken).AsTask();

    public Task<DictamenTecnico?> GetByRequestAsync(Guid requestId, CancellationToken cancellationToken = default) =>
        _dbContext.DictamenesTecnicos.FirstOrDefaultAsync(dictamen => dictamen.SolicitudId == requestId, cancellationToken);
}
