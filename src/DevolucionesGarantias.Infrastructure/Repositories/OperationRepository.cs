using DevolucionesGarantias.Application.Operation.Interfaces;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Infrastructure.Repositories;

public sealed class OperationRepository : IOperationRepository
{
    private readonly AppDbContext _dbContext;

    public OperationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Solicitud>> ListAsync(EstadoSolicitudEnum? status = null, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Solicitudes
            .Include(request => request.Evidencias)
            .Include(request => request.Timeline)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(request => request.EstadoActual == status.Value);
        }

        return await query
            .OrderByDescending(request => request.CreatedAt)
            .ToArrayAsync(cancellationToken);
    }

    public Task<Solicitud?> GetByIdAsync(Guid requestId, CancellationToken cancellationToken = default) =>
        _dbContext.Solicitudes
            .Include(request => request.Evidencias)
            .Include(request => request.Timeline)
            .FirstOrDefaultAsync(request => request.Id == requestId, cancellationToken);

    public Task<Proveedor?> GetProviderByIdAsync(Guid providerId, CancellationToken cancellationToken = default) =>
        _dbContext.Proveedores
            .FirstOrDefaultAsync(provider => provider.Id == providerId, cancellationToken);

    public Task<Proveedor?> GetDefaultProviderAsync(CancellationToken cancellationToken = default) =>
        _dbContext.Proveedores
            .OrderBy(provider => provider.Nombre)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<CasoAsignado?> GetAssignedCaseByRequestAsync(Guid requestId, CancellationToken cancellationToken = default) =>
        _dbContext.CasosAsignados
            .FirstOrDefaultAsync(assignedCase => assignedCase.SolicitudId == requestId, cancellationToken);

    public Task<ValidacionGarantia?> GetWarrantyValidationByRequestAsync(Guid requestId, CancellationToken cancellationToken = default) =>
        _dbContext.ValidacionesGarantia
            .FirstOrDefaultAsync(validation => validation.SolicitudId == requestId, cancellationToken);

    public Task<DictamenTecnico?> GetTechnicalReportByRequestAsync(Guid requestId, CancellationToken cancellationToken = default) =>
        _dbContext.DictamenesTecnicos
            .FirstOrDefaultAsync(report => report.SolicitudId == requestId, cancellationToken);

    public Task AddAssignedCaseAsync(CasoAsignado assignedCase, CancellationToken cancellationToken = default) =>
        _dbContext.CasosAsignados.AddAsync(assignedCase, cancellationToken).AsTask();

    public Task AddInformationRequestAsync(SolicitudInformacionAdicional informationRequest, CancellationToken cancellationToken = default) =>
        _dbContext.SolicitudesInformacionAdicional.AddAsync(informationRequest, cancellationToken).AsTask();
}
