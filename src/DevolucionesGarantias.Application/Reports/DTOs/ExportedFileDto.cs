using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Reports.DTOs;

public sealed record ExportedFileDto(
    Guid Id,
    Guid ReportId,
    FormatoReporte Format,
    string Bucket,
    string Path,
    string? Url,
    string FileName,
    DateTimeOffset ExportedAt);
