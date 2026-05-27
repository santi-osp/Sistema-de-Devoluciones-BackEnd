namespace DevolucionesGarantias.Application.Providers.Interfaces;

public interface IInventarioService
{
    Task<bool> HasReplacementStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default);
}
