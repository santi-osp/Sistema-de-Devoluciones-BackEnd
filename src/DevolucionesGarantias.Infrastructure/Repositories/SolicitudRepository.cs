using DevolucionesGarantias.Application.Requests.Interfaces;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Infrastructure.Repositories;

public sealed class SolicitudRepository : ISolicitudRepository
{
    private readonly AppDbContext _dbContext;

    public SolicitudRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Solicitud?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _dbContext.Solicitudes
            .Include(request => request.Evidencias)
            .Include(request => request.Timeline)
            .FirstOrDefaultAsync(request => request.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<Solicitud>> ListByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default) =>
        await _dbContext.Solicitudes
            .Include(request => request.Evidencias)
            .Include(request => request.Timeline)
            .Where(request => request.ClienteId == customerId)
            .OrderByDescending(request => request.CreatedAt)
            .ToArrayAsync(cancellationToken);

    public Task<bool> ExistsDuplicateAsync(Guid customerId, Guid orderId, Guid productId, string reason, TipoSolicitud type, CancellationToken cancellationToken = default) =>
        _dbContext.Solicitudes.AnyAsync(
            request =>
                request.ClienteId == customerId &&
                request.PedidoId == orderId &&
                request.ProductoId == productId &&
                request.Tipo == type &&
                request.Motivo == reason.Trim(),
            cancellationToken);

    public Task AddAsync(Solicitud solicitud, CancellationToken cancellationToken = default) =>
        _dbContext.Solicitudes.AddAsync(solicitud, cancellationToken).AsTask();
}
