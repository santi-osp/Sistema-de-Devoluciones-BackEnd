using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Application.Operation.Interfaces;

public interface IComentarioRepository
{
    Task<IReadOnlyCollection<ComentarioInterno>> ListByRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task AddAsync(ComentarioInterno comment, CancellationToken cancellationToken = default);
}
