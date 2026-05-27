using DevolucionesGarantias.Application.Auth.DTOs;
using DevolucionesGarantias.Application.Auth.Interfaces;
using DevolucionesGarantias.Application.Auth.Validators;
using DevolucionesGarantias.Application.Common.DTOs;
using DevolucionesGarantias.Application.Common.Interfaces;
using DevolucionesGarantias.Application.Common.Responses;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.ValueObjects;

namespace DevolucionesGarantias.Application.Auth.Services;

public sealed class AuthService
{
    private readonly IUsuarioRepository _users;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokens;
    private readonly ISessionStore _sessions;
    private readonly IAutorizacionPolicy _authorizationPolicy;
    private readonly IUnitOfWork _unitOfWork;
    private readonly LoginRequestValidator _validator;

    public AuthService(
        IUsuarioRepository users,
        IPasswordHasher passwordHasher,
        ITokenService tokens,
        ISessionStore sessions,
        IAutorizacionPolicy authorizationPolicy,
        IUnitOfWork unitOfWork,
        LoginRequestValidator validator)
    {
        _users = users;
        _passwordHasher = passwordHasher;
        _tokens = tokens;
        _sessions = sessions;
        _authorizationPolicy = authorizationPolicy;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<ApplicationResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var validationErrors = _validator.Validate(request.Email, request.Password);
        if (validationErrors.Count > 0)
        {
            return ApplicationResponse<AuthResponseDto>.Failure(validationErrors.Select(error => new ErrorDto("auth.validation", error)));
        }

        var email = new Email(request.Email);
        var user = await _users.GetByEmailAsync(email, cancellationToken);
        var isValid = user is not null && _passwordHasher.Verify(request.Password, user.PasswordHash);

        await _users.AddLoginAttemptAsync(
            new LoginAttempt(email, isValid, isValid ? null : "Credenciales invalidas.", request.IpAddress, request.UserAgent),
            cancellationToken);

        if (!isValid || user is null)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApplicationResponse<AuthResponseDto>.Failure([new ErrorDto("auth.invalid_credentials", "Credenciales invalidas.")]);
        }

        var token = await _tokens.CreateAccessTokenAsync(user, cancellationToken);
        await _sessions.CreateAsync(user.Id, token.Token, token.ExpiresAt, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new AuthResponseDto(
            user.Id,
            user.Nombre,
            user.Correo.Value,
            user.Roles.Select(role => role.Nombre).ToArray(),
            token.Token,
            token.ExpiresAt);

        return ApplicationResponse<AuthResponseDto>.Success(response);
    }

    public async Task LogoutAsync(LogoutRequestDto request, CancellationToken cancellationToken = default)
    {
        await _sessions.RevokeAsync(request.SessionId, request.UserId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<AccessValidationDto> ValidateAccessAsync(Guid userId, string requiredRole, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return new AccessValidationDto(userId, requiredRole, false, "Usuario no encontrado.");
        }

        var isAllowed = _authorizationPolicy.CanAccess(user, requiredRole);
        return new AccessValidationDto(userId, requiredRole, isAllowed, isAllowed ? null : "Acceso denegado.");
    }

    public async Task<CurrentUserDto?> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(userId, cancellationToken);
        return user is null ? null : ToCurrentUser(user);
    }

    public CurrentUserDto ToCurrentUser(Usuario user) =>
        new(user.Id, user.Nombre, user.Correo.Value, user.Roles.Select(role => role.Nombre).ToArray());
}
