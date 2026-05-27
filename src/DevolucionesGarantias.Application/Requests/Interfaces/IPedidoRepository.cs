using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Application.Requests.Interfaces;

public interface IPedidoRepository
{
    Task<Pedido?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Pedido>> ListByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);
}
