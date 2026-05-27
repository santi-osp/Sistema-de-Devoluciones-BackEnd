using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Application.Auth.Interfaces;

public interface ISessionStore
{
    Task<Sesion> CreateAsync(Guid userId, string accessToken, DateTimeOffset expiresAt, CancellationToken cancellationToken = default);
    Task RevokeAsync(Guid sessionId, Guid userId, CancellationToken cancellationToken = default);
}
