using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Reports.DTOs;

public sealed record ExportReportRequestDto(Guid ReportId, FormatoReporte Format);
