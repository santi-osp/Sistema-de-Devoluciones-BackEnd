using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Requests.DTOs;

public sealed record CreateRequestDto(
    Guid CustomerId,
    Guid OrderId,
    Guid ProductId,
    TipoSolicitud Type,
    string Reason,
    string Description,
    int Quantity,
    PreferenciaSolucion PreferredSolution,
    IReadOnlyCollection<UploadEvidenceDto>? Evidence = null);
