using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Providers.DTOs;

public sealed record TechnicalReportResultDto(
    Guid Id,
    Guid RequestId,
    ResultadoDictamen Result,
    string TechnicalReason,
    string Observations,
    DateTimeOffset IssuedAt);
