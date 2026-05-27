using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Providers.DTOs;

public sealed record ProviderDecisionDto(Guid RequestId, PreferenciaSolucion PreferredSolution, ResultadoDictamen TechnicalResult);
