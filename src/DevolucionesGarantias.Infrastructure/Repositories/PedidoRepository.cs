using DevolucionesGarantias.Application.Requests.Interfaces;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Infrastructure.Repositories;

public sealed class PedidoRepository : IPedidoRepository
{
    private readonly AppDbContext _dbContext;

    public PedidoRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Pedido?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _dbContext.Pedidos
            .Include(order => order.Productos)
            .FirstOrDefaultAsync(order => order.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<Pedido>> ListByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default) =>
        await _dbContext.Pedidos
            .Include(order => order.Productos)
            .Where(order => order.ClienteId == customerId)
            .OrderByDescending(order => order.FechaCompra)
            .ToArrayAsync(cancellationToken);
}
