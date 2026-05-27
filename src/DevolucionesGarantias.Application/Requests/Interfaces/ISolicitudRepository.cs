using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Requests.Interfaces;

public interface ISolicitudRepository
{
    Task<Solicitud?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Solicitud>> ListByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<bool> ExistsDuplicateAsync(Guid customerId, Guid orderId, Guid productId, string reason, TipoSolicitud type, CancellationToken cancellationToken = default);
    Task AddAsync(Solicitud solicitud, CancellationToken cancellationToken = default);
}
