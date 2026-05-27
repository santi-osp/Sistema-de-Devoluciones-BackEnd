namespace DevolucionesGarantias.Application.Auth.DTOs;

public sealed record LogoutRequestDto(Guid SessionId, Guid UserId);
