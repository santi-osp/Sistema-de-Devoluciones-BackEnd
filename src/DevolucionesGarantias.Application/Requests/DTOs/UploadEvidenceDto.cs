using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Requests.DTOs;

public sealed record UploadEvidenceDto(
    Guid RequestId,
    TipoEvidencia Type,
    string FileName,
    string ContentType,
    Stream Content);
