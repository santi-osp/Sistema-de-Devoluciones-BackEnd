using DevolucionesGarantias.Persistence.Context;
using DevolucionesGarantias.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevolucionesGarantias.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("NeonPostgres");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("ConnectionStrings:NeonPostgres must be configured.");
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<DatabaseSeeder>();
        services.AddScoped<RoleSeeder>();
        services.AddScoped<UserSeeder>();
        services.AddScoped<RequestCatalogSeeder>();
        services.AddScoped<OrderProductSeeder>();
        services.AddScoped<RequestSeeder>();
        services.AddScoped<ReportSeeder>();

        return services;
    }
}
