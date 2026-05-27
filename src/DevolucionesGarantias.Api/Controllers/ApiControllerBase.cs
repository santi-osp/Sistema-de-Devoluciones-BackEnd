using System.Security.Claims;
using DevolucionesGarantias.Application.Common.Security;
using Microsoft.AspNetCore.Mvc;

namespace DevolucionesGarantias.Api.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected Guid CurrentUserId
    {
        get
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(value, out var userId) ? userId : Guid.Empty;
        }
    }

    protected bool IsCliente => User.IsInRole(RoleNames.Cliente);
    protected bool IsProveedor => User.IsInRole(RoleNames.Proveedor);
    protected bool IsBackOffice => User.IsInRole(RoleNames.Administrador) || User.IsInRole(RoleNames.Analista);
}
