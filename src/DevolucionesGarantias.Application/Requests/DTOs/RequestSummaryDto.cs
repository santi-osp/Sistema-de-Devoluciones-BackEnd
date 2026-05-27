using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Requests.DTOs;

public sealed record RequestSummaryDto(
    Guid Id,
    Guid CustomerId,
    Guid OrderId,
    Guid ProductId,
    TipoSolicitud Type,
    EstadoSolicitudEnum Status,
    string Reason,
    DateTimeOffset CreatedAt);
