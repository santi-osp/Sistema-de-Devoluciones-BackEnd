namespace DevolucionesGarantias.Application.Auth.DTOs;

public sealed record CurrentUserDto(Guid UserId, string Name, string Email, IReadOnlyCollection<string> Roles);
