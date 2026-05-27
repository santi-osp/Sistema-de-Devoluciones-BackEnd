using DevolucionesGarantias.Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevolucionesGarantias.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
[ApiExplorerSettings(GroupName = "Health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse<object>> Get()
    {
        var payload = new
        {
            service = "DevolucionesGarantias.Api",
            status = "Healthy",
            ready = true,
            timestampUtc = DateTimeOffset.UtcNow
        };

        return Ok(ApiResponse<object>.Ok(payload, "Backend base disponible."));
    }
}
