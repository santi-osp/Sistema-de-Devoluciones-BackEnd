using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Operation.DTOs;

public sealed record OperationRequestDto(
    Guid Id,
    Guid CustomerId,
    TipoSolicitud Type,
    EstadoSolicitudEnum Status,
    string Reason,
    DateTimeOffset CreatedAt);
