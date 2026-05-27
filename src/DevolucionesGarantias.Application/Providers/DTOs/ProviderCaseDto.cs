using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Providers.DTOs;

public sealed record ProviderCaseDto(
    Guid Id,
    Guid RequestId,
    Guid ProviderId,
    EstadoAsignacionProveedor Status,
    DateTimeOffset AssignedAt);
