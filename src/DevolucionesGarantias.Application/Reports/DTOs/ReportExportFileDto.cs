using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Reports.DTOs;

public sealed record ReportExportFileDto(
    Guid ReportId,
    FormatoReporte Format,
    string FileName,
    string ContentType,
    byte[] Content,
    ExportedFileDto StoredFile);
