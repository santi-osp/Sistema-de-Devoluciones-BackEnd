using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Application.Providers.Interfaces;

public interface IGarantiaService
{
    Task<bool> IsWarrantyActiveAsync(Producto product, DateTimeOffset evaluationDate, CancellationToken cancellationToken = default);
}
