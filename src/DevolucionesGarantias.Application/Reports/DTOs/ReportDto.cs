namespace DevolucionesGarantias.Application.Reports.DTOs;

public sealed record ReportDto(
    Guid Id,
    string Title,
    ReportFilterDto Filter,
    string GeneratedBy,
    DateTimeOffset GeneratedAt,
    IReadOnlyCollection<MetricDto> Metrics,
    IReadOnlyCollection<ExportedFileDto> ExportedFiles);
