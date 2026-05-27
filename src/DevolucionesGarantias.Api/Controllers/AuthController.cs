using DevolucionesGarantias.Application.Auth.DTOs;
using DevolucionesGarantias.Application.Auth.Services;
using DevolucionesGarantias.Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevolucionesGarantias.Api.Controllers;

[Route("api/auth")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "Auth")]
public sealed class AuthController : ApiControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _authService.LoginAsync(request with
        {
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
            UserAgent = Request.Headers.UserAgent.ToString()
        }, cancellationToken);

        return response.Succeeded ? Ok(ApiResponse<AuthResponseDto>.Ok(response.Data!)) : BadRequest(response);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(LogoutRequestDto request, CancellationToken cancellationToken)
    {
        await _authService.LogoutAsync(request with { UserId = CurrentUserId }, cancellationToken);
        return Ok(ApiResponse<object>.Ok(new { loggedOut = true }));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<CurrentUserDto>>> Me(CancellationToken cancellationToken)
    {
        var user = await _authService.GetCurrentUserAsync(CurrentUserId, cancellationToken);
        return user is null ? NotFound(ApiResponse<object>.Fail("Usuario no encontrado.")) : Ok(ApiResponse<CurrentUserDto>.Ok(user));
    }

    [HttpGet("validate-access")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<AccessValidationDto>>> ValidateAccess([FromQuery] string requiredRole, CancellationToken cancellationToken)
    {
        var result = await _authService.ValidateAccessAsync(CurrentUserId, requiredRole, cancellationToken);
        return Ok(ApiResponse<AccessValidationDto>.Ok(result));
    }
}
