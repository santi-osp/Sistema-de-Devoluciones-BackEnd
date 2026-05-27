using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Application.Providers.Interfaces;

public interface IProviderCaseRepository
{
    Task<IReadOnlyCollection<CasoAsignado>> ListByProviderAsync(Guid providerId, CancellationToken cancellationToken = default);
    Task<CasoAsignado?> GetAssignedCaseAsync(Guid providerId, Guid requestId, CancellationToken cancellationToken = default);
    Task<Solicitud?> GetRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task AddWarrantyValidationAsync(ValidacionGarantia validation, CancellationToken cancellationToken = default);
    Task AddProductReceptionAsync(RecepcionProducto reception, CancellationToken cancellationToken = default);
}
