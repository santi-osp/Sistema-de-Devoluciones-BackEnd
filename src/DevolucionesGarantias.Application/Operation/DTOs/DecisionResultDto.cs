namespace DevolucionesGarantias.Application.Operation.DTOs;

public sealed record DecisionResultDto(Guid DecisionId, Guid RequestId, bool Approved, string Reason, DateTimeOffset DecidedAt);
