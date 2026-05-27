using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Application.Auth.Interfaces;

public interface ITokenService
{
    Task<TokenDescriptor> CreateAccessTokenAsync(Usuario user, CancellationToken cancellationToken = default);
}

public sealed record TokenDescriptor(string Token, DateTimeOffset ExpiresAt);
