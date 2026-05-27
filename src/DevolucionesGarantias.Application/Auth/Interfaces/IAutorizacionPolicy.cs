using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Application.Auth.Interfaces;

public interface IAutorizacionPolicy
{
    bool HasRole(Usuario user, string roleName);
    bool CanAccess(Usuario user, string requiredRole);
}
