namespace DevolucionesGarantias.Application.Auth.DTOs;

public sealed record AuthResponseDto(
    Guid UserId,
    string Name,
    string Email,
    IReadOnlyCollection<string> Roles,
    string AccessToken,
    DateTimeOffset ExpiresAt);
