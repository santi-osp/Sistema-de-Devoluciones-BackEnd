using Microsoft.Extensions.DependencyInjection;
using DevolucionesGarantias.Application.Auth.Services;
using DevolucionesGarantias.Application.Auth.Validators;
using DevolucionesGarantias.Application.Operation.Services;
using DevolucionesGarantias.Application.Operation.Validators;
using DevolucionesGarantias.Application.Providers.Services;
using DevolucionesGarantias.Application.Providers.Validators;
using DevolucionesGarantias.Application.Reports.Services;
using DevolucionesGarantias.Application.Reports.Validators;
using DevolucionesGarantias.Application.Requests.Services;
using DevolucionesGarantias.Application.Requests.Validators;

namespace DevolucionesGarantias.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<OrdersService>();
        services.AddScoped<RequestsService>();
        services.AddScoped<EvidenceService>();
        services.AddScoped<BandejaOperativaService>();
        services.AddScoped<RevisionOperativaService>();
        services.AddScoped<GestionProveedorService>();
        services.AddScoped<RecepcionProductoService>();
        services.AddScoped<ReporteService>();

        services.AddSingleton<LoginRequestValidator>();
        services.AddSingleton<CreateRequestValidator>();
        services.AddSingleton<UploadEvidenceValidator>();
        services.AddSingleton<CreateCommentValidator>();
        services.AddSingleton<DecisionValidator>();
        services.AddSingleton<RequestInformationValidator>();
        services.AddSingleton<WarrantyValidationValidator>();
        services.AddSingleton<TechnicalReportValidator>();
        services.AddSingleton<ProductReceptionValidator>();
        services.AddSingleton<ReportFilterValidator>();

        return services;
    }
}
