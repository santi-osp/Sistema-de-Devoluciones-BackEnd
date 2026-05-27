namespace DevolucionesGarantias.Application.Operation.DTOs;

public sealed record CreateCommentDto(Guid RequestId, string Text, bool VisibleToCustomer = false);
