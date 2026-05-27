using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Requests.DTOs;

public sealed record RequestDetailDto(
    Guid Id,
    Guid CustomerId,
    Guid OrderId,
    Guid ProductId,
    TipoSolicitud Type,
    string Reason,
    string Description,
    int Quantity,
    PreferenciaSolucion PreferredSolution,
    EstadoSolicitudEnum Status,
    DateTimeOffset CreatedAt,
    IReadOnlyCollection<EvidenceDto> Evidence,
    IReadOnlyCollection<RequestTimelineDto> Timeline);
