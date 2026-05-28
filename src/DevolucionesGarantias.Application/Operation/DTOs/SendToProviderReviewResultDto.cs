using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Operation.DTOs;

public sealed record SendToProviderReviewResultDto(
    Guid RequestId,
    Guid ProviderId,
    EstadoSolicitudEnum RequestStatus,
    EstadoAsignacionProveedor AssignmentStatus);
