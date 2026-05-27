using DevolucionesGarantias.Application.Providers.Interfaces;
using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Infrastructure.ExternalServices;

public sealed class GarantiaService : IGarantiaService
{
    public Task<bool> IsWarrantyActiveAsync(Producto product, DateTimeOffset evaluationDate, CancellationToken cancellationToken = default) =>
        Task.FromResult(product.GarantiaHasta >= evaluationDate);
}
