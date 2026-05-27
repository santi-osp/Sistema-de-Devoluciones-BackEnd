using DevolucionesGarantias.Application.Requests.DTOs;
using DevolucionesGarantias.Application.Requests.Interfaces;
using DevolucionesGarantias.Application.Requests.Mappings;

namespace DevolucionesGarantias.Application.Requests.Services;

public sealed class OrdersService
{
    private readonly IPedidoRepository _orders;

    public OrdersService(IPedidoRepository orders)
    {
        _orders = orders;
    }

    public async Task<IReadOnlyCollection<OrderDto>> ListByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var orders = await _orders.ListByCustomerAsync(customerId, cancellationToken);
        return orders.Select(order => order.ToDto()).ToArray();
    }

    public async Task<OrderDto?> GetAsync(Guid orderId, Guid customerId, CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(orderId, cancellationToken);
        return order is null || order.ClienteId != customerId ? null : order.ToDto();
    }
}
