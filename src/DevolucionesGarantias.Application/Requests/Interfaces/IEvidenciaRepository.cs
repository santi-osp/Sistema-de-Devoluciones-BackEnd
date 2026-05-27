using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Application.Requests.Interfaces;

public interface IEvidenciaRepository
{
    Task<IReadOnlyCollection<Evidencia>> ListByRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task AddAsync(Evidencia evidencia, CancellationToken cancellationToken = default);
}
