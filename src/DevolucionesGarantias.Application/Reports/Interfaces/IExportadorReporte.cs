using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Reports.Interfaces;

public interface IExportadorReporte
{
    FormatoReporte Format { get; }
    Task<GeneratedReportFile> ExportAsync(Reporte report, CancellationToken cancellationToken = default);
}

public sealed record GeneratedReportFile(
    FormatoReporte Format,
    string FileName,
    string ContentType,
    byte[] Content);
