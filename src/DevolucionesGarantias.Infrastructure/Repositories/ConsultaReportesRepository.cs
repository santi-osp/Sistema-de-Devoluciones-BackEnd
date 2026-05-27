using DevolucionesGarantias.Application.Reports.Interfaces;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Infrastructure.Repositories;

public sealed class ConsultaReportesRepository : IConsultaReportesRepository
{
    private readonly AppDbContext _dbContext;

    public ConsultaReportesRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<IndicadorMetrica>> CalculateMetricsAsync(FiltroReporte filter, Guid reportId, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter(_dbContext.Solicitudes.AsNoTracking(), filter);

        var total = await query.CountAsync(cancellationToken);
        var approved = await query.CountAsync(request => request.EstadoActual == EstadoSolicitudEnum.Aprobada, cancellationToken);
        var rejected = await query.CountAsync(request => request.EstadoActual == EstadoSolicitudEnum.Rechazada, cancellationToken);
        var pending = await query.CountAsync(request => request.EstadoActual == EstadoSolicitudEnum.PendienteInformacion, cancellationToken);

        return
        [
            new IndicadorMetrica(reportId, "Total solicitudes", total, "casos"),
            new IndicadorMetrica(reportId, "Solicitudes aprobadas", approved, "casos"),
            new IndicadorMetrica(reportId, "Solicitudes rechazadas", rejected, "casos"),
            new IndicadorMetrica(reportId, "Pendientes de informacion", pending, "casos")
        ];
    }

    public Task<Reporte?> GetByIdAsync(Guid reportId, CancellationToken cancellationToken = default) =>
        _dbContext.Reportes
            .Include(report => report.Metricas)
            .Include(report => report.Archivos)
            .FirstOrDefaultAsync(report => report.Id == reportId, cancellationToken);

    public async Task<IReadOnlyCollection<Reporte>> ListAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.Reportes
            .Include(report => report.Metricas)
            .Include(report => report.Archivos)
            .OrderByDescending(report => report.GeneratedAt)
            .ToArrayAsync(cancellationToken);

    public Task AddAsync(Reporte report, CancellationToken cancellationToken = default) =>
        _dbContext.Reportes.AddAsync(report, cancellationToken).AsTask();

    public Task AddExportedFileAsync(ArchivoExportado file, CancellationToken cancellationToken = default) =>
        _dbContext.ArchivosExportados.AddAsync(file, cancellationToken).AsTask();

    private static IQueryable<Solicitud> ApplyFilter(IQueryable<Solicitud> query, FiltroReporte filter)
    {
        if (filter.Periodo is not null)
        {
            query = query.Where(request => request.CreatedAt >= filter.Periodo.Start && request.CreatedAt <= filter.Periodo.End);
        }

        if (filter.Estado.HasValue)
        {
            query = query.Where(request => request.EstadoActual == filter.Estado.Value);
        }

        if (filter.TipoSolicitud.HasValue)
        {
            query = query.Where(request => request.Tipo == filter.TipoSolicitud.Value);
        }

        return query;
    }
}
