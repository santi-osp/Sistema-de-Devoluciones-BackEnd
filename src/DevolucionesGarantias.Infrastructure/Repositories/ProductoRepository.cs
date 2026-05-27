using DevolucionesGarantias.Application.Requests.Interfaces;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Infrastructure.Repositories;

public sealed class ProductoRepository : IProductoRepository
{
    private readonly AppDbContext _dbContext;

    public ProductoRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Producto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _dbContext.Productos.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
}
