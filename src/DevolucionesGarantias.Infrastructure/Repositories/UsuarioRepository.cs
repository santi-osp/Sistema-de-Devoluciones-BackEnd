using DevolucionesGarantias.Application.Auth.Interfaces;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.ValueObjects;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Infrastructure.Repositories;

public sealed class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _dbContext;

    public UsuarioRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Usuario?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default) =>
        _dbContext.Usuarios
            .Include(user => user.Roles)
            .Include(user => user.Sesiones)
            .FirstOrDefaultAsync(user => user.Correo == email, cancellationToken);

    public Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _dbContext.Usuarios
            .Include(user => user.Roles)
            .Include(user => user.Sesiones)
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);

    public Task AddLoginAttemptAsync(LoginAttempt attempt, CancellationToken cancellationToken = default) =>
        _dbContext.LoginAttempts.AddAsync(attempt, cancellationToken).AsTask();
}
