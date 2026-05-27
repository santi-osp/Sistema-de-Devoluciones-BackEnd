using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Application.Reports.Interfaces;

public interface IConsultaReportesRepository
{
    Task<IReadOnlyCollection<IndicadorMetrica>> CalculateMetricsAsync(FiltroReporte filter, Guid reportId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Reporte>> ListAsync(CancellationToken cancellationToken = default);
    Task<Reporte?> GetByIdAsync(Guid reportId, CancellationToken cancellationToken = default);
    Task AddAsync(Reporte report, CancellationToken cancellationToken = default);
    Task AddExportedFileAsync(ArchivoExportado file, CancellationToken cancellationToken = default);
}
