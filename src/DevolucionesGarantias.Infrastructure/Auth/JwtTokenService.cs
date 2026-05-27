using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevolucionesGarantias.Application.Auth.Interfaces;
using DevolucionesGarantias.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DevolucionesGarantias.Infrastructure.Auth;

public sealed class JwtTokenService : ITokenService
{
    private readonly JwtOptions _options;

    public JwtTokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public Task<TokenDescriptor> CreateAccessTokenAsync(Usuario user, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.Secret) || _options.Secret.Contains("YOUR_", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Jwt:Secret no esta configurado con un valor seguro.");
        }

        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(Math.Max(_options.AccessTokenExpirationMinutes, 1));
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Correo.Value),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Nombre)
        };

        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Nombre)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        return Task.FromResult(new TokenDescriptor(new JwtSecurityTokenHandler().WriteToken(token), expiresAt));
    }
}
