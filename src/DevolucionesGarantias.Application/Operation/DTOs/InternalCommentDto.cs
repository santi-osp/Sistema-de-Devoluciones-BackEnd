namespace DevolucionesGarantias.Application.Operation.DTOs;

public sealed record InternalCommentDto(
    Guid Id,
    Guid RequestId,
    string Text,
    string Author,
    bool VisibleToCustomer,
    DateTimeOffset CreatedAt);
