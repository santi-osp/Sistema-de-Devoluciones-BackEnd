using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Requests.DTOs;

public sealed record RequestTimelineDto(
    Guid Id,
    Guid RequestId,
    string Event,
    EstadoSolicitudEnum? PreviousStatus,
    EstadoSolicitudEnum? NewStatus,
    DateTimeOffset CreatedAt,
    string? CreatedBy);
