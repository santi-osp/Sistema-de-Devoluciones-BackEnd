using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Application.Requests.Interfaces;

public interface IProductoRepository
{
    Task<Producto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
