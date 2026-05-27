namespace DevolucionesGarantias.Application.Reports.DTOs;

public sealed record ReportSummaryDto(Guid Id, string Title, string GeneratedBy, DateTimeOffset GeneratedAt, int MetricCount);
