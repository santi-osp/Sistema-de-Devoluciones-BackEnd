using DevolucionesGarantias.Application.Requests.DTOs;
using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Operation.DTOs;

public sealed record OperationRequestDetailDto(
    Guid Id,
    Guid CustomerId,
    Guid OrderId,
    Guid ProductId,
    TipoSolicitud Type,
    EstadoSolicitudEnum Status,
    string Reason,
    string Description,
    int Quantity,
    IReadOnlyCollection<EvidenceDto> Evidence,
    IReadOnlyCollection<InternalCommentDto> Comments);
