using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.ValueObjects;

namespace DevolucionesGarantias.Application.Auth.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddLoginAttemptAsync(LoginAttempt attempt, CancellationToken cancellationToken = default);
}
