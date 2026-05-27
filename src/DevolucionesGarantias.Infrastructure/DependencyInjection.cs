using DevolucionesGarantias.Application.Auth.Interfaces;
using DevolucionesGarantias.Application.Common.Interfaces;
using DevolucionesGarantias.Application.Operation.Interfaces;
using DevolucionesGarantias.Application.Providers.Interfaces;
using DevolucionesGarantias.Application.Reports.Interfaces;
using DevolucionesGarantias.Application.Requests.Interfaces;
using DevolucionesGarantias.Infrastructure.Audit;
using DevolucionesGarantias.Infrastructure.Auth;
using DevolucionesGarantias.Infrastructure.ExternalServices;
using DevolucionesGarantias.Infrastructure.Notifications;
using DevolucionesGarantias.Infrastructure.Reports;
using DevolucionesGarantias.Infrastructure.Repositories;
using DevolucionesGarantias.Infrastructure.Storage;
using DevolucionesGarantias.Infrastructure.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevolucionesGarantias.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<SupabaseStorageOptions>(configuration.GetSection("Supabase"));
        services.Configure<FileValidationOptions>(configuration.GetSection("FileValidation"));

        services.AddHttpContextAccessor();

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IProductoRepository, ProductoRepository>();
        services.AddScoped<ISolicitudRepository, SolicitudRepository>();
        services.AddScoped<IEvidenciaRepository, EvidenciaRepository>();
        services.AddScoped<IOperationRepository, OperationRepository>();
        services.AddScoped<IComentarioRepository, ComentarioRepository>();
        services.AddScoped<IDecisionOperativaRepository, DecisionOperativaRepository>();
        services.AddScoped<IProviderCaseRepository, ProviderCaseRepository>();
        services.AddScoped<IDictamenRepository, DictamenRepository>();
        services.AddScoped<IConsultaReportesRepository, ConsultaReportesRepository>();
        services.AddScoped<AuditLogRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ISessionStore, SessionStore>();
        services.AddScoped<IAutorizacionPolicy, AuthorizationPolicyService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IAuditService, AuditService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<IGarantiaService, GarantiaService>();
        services.AddScoped<IInventarioService, InMemoryInventarioService>();
        services.AddSingleton<INotificationService, InMemoryNotificationService>();
        services.AddScoped<IExportadorReporte, CsvReportExporter>();
        services.AddScoped<IExportadorReporte, PdfReportExporter>();

        services.AddHttpClient<IFileStorageService, SupabaseStorageService>();

        return services;
    }
}
