using DevolucionesGarantias.Application;
using DevolucionesGarantias.Domain;
using DevolucionesGarantias.Infrastructure;
using DevolucionesGarantias.Persistence;
using DevolucionesGarantias.Shared;

namespace DevolucionesGarantias.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddApiSwagger();
        services.AddApiCors(configuration);
        services.AddApiAuthentication(configuration);

        services.AddDomain();
        services.AddApplication();
        services.AddInfrastructure(configuration);
        services.AddPersistence(configuration);
        services.AddShared();

        return services;
    }
}

