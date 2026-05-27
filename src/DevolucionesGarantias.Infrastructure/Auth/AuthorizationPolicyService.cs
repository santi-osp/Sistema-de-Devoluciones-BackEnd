using DevolucionesGarantias.Application.Auth.Interfaces;
using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Infrastructure.Auth;

public sealed class AuthorizationPolicyService : IAutorizacionPolicy
{
    public bool HasRole(Usuario user, string roleName) =>
        user.Roles.Any(role => role.Nombre.Equals(roleName, StringComparison.OrdinalIgnoreCase));

    public bool CanAccess(Usuario user, string requiredRole) => HasRole(user, requiredRole);
}
