using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Requests.DTOs;

public sealed record EvidenceDto(
    Guid Id,
    Guid RequestId,
    TipoEvidencia Type,
    string Bucket,
    string Path,
    string? Url,
    string FileName,
    long SizeInBytes,
    DateTimeOffset UploadedAt);
