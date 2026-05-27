namespace DevolucionesGarantias.Application.Auth.DTOs;

public sealed record AccessValidationDto(Guid UserId, string RequiredRole, bool IsAllowed, string? Reason = null);
