namespace DevolucionesGarantias.Application.Operation.DTOs;

public sealed record DecisionDto(Guid RequestId, bool Approved, string Reason);
