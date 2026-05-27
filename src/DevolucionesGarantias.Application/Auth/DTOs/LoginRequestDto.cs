namespace DevolucionesGarantias.Application.Auth.DTOs;

public sealed record LoginRequestDto(string Email, string Password, string? IpAddress = null, string? UserAgent = null);
