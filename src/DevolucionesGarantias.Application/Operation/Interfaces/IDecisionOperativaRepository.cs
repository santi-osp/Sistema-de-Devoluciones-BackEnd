using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Application.Operation.Interfaces;

public interface IDecisionOperativaRepository
{
    Task AddAsync(DecisionOperativa decision, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<DecisionOperativa>> ListByRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
}
