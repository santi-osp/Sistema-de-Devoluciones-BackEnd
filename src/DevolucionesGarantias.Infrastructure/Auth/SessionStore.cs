using DevolucionesGarantias.Application.Auth.Interfaces;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Infrastructure.Auth;

public sealed class SessionStore : ISessionStore
{
    private readonly AppDbContext _dbContext;

    public SessionStore(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Sesion> CreateAsync(Guid userId, string accessToken, DateTimeOffset expiresAt, CancellationToken cancellationToken = default)
    {
        var session = new Sesion(userId, accessToken, expiresAt);
        await _dbContext.Sesiones.AddAsync(session, cancellationToken);
        return session;
    }

    public async Task RevokeAsync(Guid sessionId, Guid userId, CancellationToken cancellationToken = default)
    {
        var session = await _dbContext.Sesiones
            .FirstOrDefaultAsync(item => item.Id == sessionId && item.UsuarioId == userId, cancellationToken);

        session?.Cerrar();
    }
}
