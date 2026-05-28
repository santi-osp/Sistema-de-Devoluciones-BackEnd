using DevolucionesGarantias.Application.Providers.Interfaces;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Infrastructure.Repositories;

public sealed class ProviderCaseRepository : IProviderCaseRepository
{
    private readonly AppDbContext _dbContext;

    public ProviderCaseRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<CasoAsignado>> ListByProviderAsync(Guid providerId, CancellationToken cancellationToken = default) =>
        await _dbContext.CasosAsignados
            .Where(assignedCase => assignedCase.ProveedorId == providerId)
            .OrderByDescending(assignedCase => assignedCase.AssignedAt)
            .ToArrayAsync(cancellationToken);

    public Task<CasoAsignado?> GetAssignedCaseAsync(Guid providerId, Guid requestId, CancellationToken cancellationToken = default) =>
        _dbContext.CasosAsignados
            .FirstOrDefaultAsync(assignedCase => assignedCase.ProveedorId == providerId && assignedCase.SolicitudId == requestId, cancellationToken);

    public Task<Solicitud?> GetRequestAsync(Guid requestId, CancellationToken cancellationToken = default) =>
        _dbContext.Solicitudes
            .Include(request => request.Evidencias)
            .Include(request => request.Timeline)
            .FirstOrDefaultAsync(request => request.Id == requestId, cancellationToken);

    public Task<ValidacionGarantia?> GetWarrantyValidationByRequestAsync(Guid requestId, CancellationToken cancellationToken = default) =>
        _dbContext.ValidacionesGarantia
            .FirstOrDefaultAsync(validation => validation.SolicitudId == requestId, cancellationToken);

    public Task AddWarrantyValidationAsync(ValidacionGarantia validation, CancellationToken cancellationToken = default) =>
        _dbContext.ValidacionesGarantia.AddAsync(validation, cancellationToken).AsTask();

    public Task AddProductReceptionAsync(RecepcionProducto reception, CancellationToken cancellationToken = default) =>
        _dbContext.RecepcionesProducto.AddAsync(reception, cancellationToken).AsTask();
}
