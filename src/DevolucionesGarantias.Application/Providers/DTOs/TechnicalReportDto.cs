using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Providers.DTOs;

public sealed record TechnicalReportDto(Guid RequestId, ResultadoDictamen Result, string TechnicalReason, string Observations);
