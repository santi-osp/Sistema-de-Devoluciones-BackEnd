using DevolucionesGarantias.Application.Providers.Interfaces;

namespace DevolucionesGarantias.Infrastructure.ExternalServices;

public sealed class InMemoryInventarioService : IInventarioService
{
    public Task<bool> HasReplacementStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default) =>
        Task.FromResult(quantity > 0);
}
