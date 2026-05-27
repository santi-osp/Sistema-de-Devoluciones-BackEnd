using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DevolucionesGarantias.Api.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];
        var secret = configuration["Jwt:Secret"];

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = !string.IsNullOrWhiteSpace(issuer),
                    ValidIssuer = issuer,
                    ValidateAudience = !string.IsNullOrWhiteSpace(audience),
                    ValidAudience = audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetJwtSecret(secret))),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2)
                };
            });

        services.AddAuthorization();

        return services;
    }

    private static string GetJwtSecret(string? secret)
    {
        if (string.IsNullOrWhiteSpace(secret) || secret.Contains("YOUR_", StringComparison.OrdinalIgnoreCase))
        {
            return "development-placeholder-secret-change-before-authentication";
        }

        return secret;
    }
}
