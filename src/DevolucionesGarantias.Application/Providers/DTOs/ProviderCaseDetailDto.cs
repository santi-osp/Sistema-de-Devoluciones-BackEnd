using DevolucionesGarantias.Application.Requests.DTOs;
using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Providers.DTOs;

public sealed record ProviderCaseDetailDto(
    Guid Id,
    Guid RequestId,
    Guid ProviderId,
    EstadoAsignacionProveedor Status,
    RequestDetailDto Request,
    ProviderReviewDto Review);
