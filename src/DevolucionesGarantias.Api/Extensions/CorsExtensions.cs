namespace DevolucionesGarantias.Api.Extensions;

public static class CorsExtensions
{
    public const string DefaultCorsPolicyName = "FrontendClients";

    public static IServiceCollection AddApiCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(DefaultCorsPolicyName, policy =>
            {
                var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

                policy.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }
}

