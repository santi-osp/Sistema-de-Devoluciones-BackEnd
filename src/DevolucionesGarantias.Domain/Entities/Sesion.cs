using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class Sesion : IEntity
{
    public Sesion(Guid usuarioId, string token, DateTimeOffset expiresAt)
    {
        if (usuarioId == Guid.Empty)
        {
            throw new BusinessRuleException("La sesion debe asociarse a un usuario.");
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new BusinessRuleException("El token de sesion es obligatorio.");
        }

        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
        Token = token.Trim();
        ExpiresAt = expiresAt;
        CreatedAt = DateTimeOffset.UtcNow;
        IsActive = true;
    }

    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public string Token { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsExpired => IsExpiredAt(DateTimeOffset.UtcNow);

    public bool IsExpiredAt(DateTimeOffset instant) => !IsActive || ExpiresAt <= instant;

    public void Cerrar()
    {
        IsActive = false;
    }
}

