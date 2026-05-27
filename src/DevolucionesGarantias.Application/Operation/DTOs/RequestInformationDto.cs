namespace DevolucionesGarantias.Application.Operation.DTOs;

public sealed record RequestInformationDto(Guid RequestId, string Message, DateTimeOffset Deadline);
