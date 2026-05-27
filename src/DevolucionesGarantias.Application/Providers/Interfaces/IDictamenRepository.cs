using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Application.Providers.Interfaces;

public interface IDictamenRepository
{
    Task AddAsync(DictamenTecnico dictamen, CancellationToken cancellationToken = default);
    Task<DictamenTecnico?> GetByRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
}
