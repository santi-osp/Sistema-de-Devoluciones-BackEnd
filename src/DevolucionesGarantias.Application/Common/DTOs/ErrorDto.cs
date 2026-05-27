namespace DevolucionesGarantias.Application.Common.DTOs;

public sealed record ErrorDto(string Code, string Message, string? Field = null);
