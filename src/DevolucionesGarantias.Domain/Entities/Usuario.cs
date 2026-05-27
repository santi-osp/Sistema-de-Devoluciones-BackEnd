using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;
using DevolucionesGarantias.Domain.ValueObjects;

namespace DevolucionesGarantias.Domain.Entities;

public abstract class Usuario : IEntity, IAuditableEntity
{
    private readonly List<Rol> _roles = [];
    private readonly List<Sesion> _sesiones = [];

    protected Usuario(string nombre, Email correo, string passwordHash, string? createdBy = null)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new BusinessRuleException("El nombre del usuario es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new BusinessRuleException("El hash de password es obligatorio.");
        }

        Id = Guid.NewGuid();
        Nombre = nombre.Trim();
        Correo = correo;
        PasswordHash = passwordHash;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    public Guid Id { get; private set; }
    public string Nombre { get; private set; }
    public Email Correo { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTimeOffset? UltimoAcceso { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }
    public IReadOnlyCollection<Rol> Roles => _roles.AsReadOnly();
    public IReadOnlyCollection<Sesion> Sesiones => _sesiones.AsReadOnly();

    public void AgregarRol(Rol rol)
    {
        if (_roles.Any(existing => existing.Tipo == rol.Tipo))
        {
            return;
        }

        _roles.Add(rol);
    }

    public void RegistrarSesion(Sesion sesion)
    {
        _sesiones.Add(sesion);
        UltimoAcceso = DateTimeOffset.UtcNow;
    }

    public void MarcarActualizacion(string? updatedBy)
    {
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = updatedBy;
    }
}

