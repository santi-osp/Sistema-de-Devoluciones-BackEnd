using DevolucionesGarantias.Application.Common.Interfaces;
using DevolucionesGarantias.Application.Providers.DTOs;
using DevolucionesGarantias.Application.Providers.Interfaces;
using DevolucionesGarantias.Application.Providers.Validators;
using DevolucionesGarantias.Application.Requests.Mappings;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Application.Providers.Services;

public sealed class GestionProveedorService
{
    private readonly IProviderCaseRepository _cases;
    private readonly IDictamenRepository _dictamenes;
    private readonly IInventarioService _inventory;
    private readonly ProviderReviewProjectionService _providerReview;
    private readonly IUnitOfWork _unitOfWork;
    private readonly WarrantyValidationValidator _warrantyValidator;
    private readonly TechnicalReportValidator _technicalReportValidator;

    public GestionProveedorService(
        IProviderCaseRepository cases,
        IDictamenRepository dictamenes,
        IInventarioService inventory,
        ProviderReviewProjectionService providerReview,
        IUnitOfWork unitOfWork,
        WarrantyValidationValidator warrantyValidator,
        TechnicalReportValidator technicalReportValidator)
    {
        _cases = cases;
        _dictamenes = dictamenes;
        _inventory = inventory;
        _providerReview = providerReview;
        _unitOfWork = unitOfWork;
        _warrantyValidator = warrantyValidator;
        _technicalReportValidator = technicalReportValidator;
    }

    public async Task<IReadOnlyCollection<ProviderCaseDto>> ListAssignedAsync(Guid providerId, CancellationToken cancellationToken = default)
    {
        var cases = await _cases.ListByProviderAsync(providerId, cancellationToken);
        return cases.Select(ToDto).ToArray();
    }

    public async Task<ProviderCaseDetailDto> GetAssignedCaseAsync(Guid providerId, Guid requestId, CancellationToken cancellationToken = default)
    {
        var assignedCase = await GetAssignedCaseOrThrow(providerId, requestId, cancellationToken);
        var request = await _cases.GetRequestAsync(requestId, cancellationToken)
            ?? throw new BusinessRuleException("Solicitud no encontrada.");
        var validation = await _cases.GetWarrantyValidationByRequestAsync(requestId, cancellationToken);
        var technicalReport = await _dictamenes.GetByRequestAsync(requestId, cancellationToken);
        var review = await _providerReview.BuildAsync(request, assignedCase, validation, technicalReport, cancellationToken);

        return new ProviderCaseDetailDto(assignedCase.Id, assignedCase.SolicitudId, assignedCase.ProveedorId, assignedCase.Estado, request.ToDetailDto(), review);
    }

    public async Task<WarrantyValidationResultDto> ValidateWarrantyAsync(Guid providerId, WarrantyValidationDto request, CancellationToken cancellationToken = default)
    {
        _warrantyValidator.ValidateAndThrow(request);
        var assignedCase = await GetAssignedCaseOrThrow(providerId, request.RequestId, cancellationToken);
        var solicitud = await _cases.GetRequestAsync(request.RequestId, cancellationToken)
            ?? throw new BusinessRuleException("Solicitud no encontrada.");

        EnsureProviderCanWork(solicitud, assignedCase);

        if (request.ProductId != solicitud.ProductoId)
        {
            throw new BusinessRuleException("El producto validado no corresponde a la solicitud.");
        }

        var existing = await _cases.GetWarrantyValidationByRequestAsync(request.RequestId, cancellationToken);
        if (existing is not null)
        {
            throw new BusinessRuleException("La validacion de garantia ya fue registrada para esta solicitud.");
        }

        var validation = new ValidacionGarantia(request.RequestId, request.ProductId, request.IsWarrantyValid, providerId.ToString(), request.Reason);
        await _cases.AddWarrantyValidationAsync(validation, cancellationToken);
        assignedCase.CambiarEstado(EstadoAsignacionProveedor.EnEvaluacion);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new WarrantyValidationResultDto(validation.Id, validation.SolicitudId, validation.ProductoId, validation.IsWarrantyValid, validation.Reason, validation.ValidatedAt);
    }

    public async Task<TechnicalReportResultDto> RegisterTechnicalReportAsync(Guid providerId, TechnicalReportDto request, CancellationToken cancellationToken = default)
    {
        _technicalReportValidator.ValidateAndThrow(request);
        var assignedCase = await GetAssignedCaseOrThrow(providerId, request.RequestId, cancellationToken);
        var solicitud = await _cases.GetRequestAsync(request.RequestId, cancellationToken)
            ?? throw new BusinessRuleException("Solicitud no encontrada.");

        EnsureProviderCanWork(solicitud, assignedCase);

        var existingReport = await _dictamenes.GetByRequestAsync(request.RequestId, cancellationToken);
        if (existingReport is not null)
        {
            throw new BusinessRuleException("El dictamen tecnico ya fue registrado para esta solicitud.");
        }

        var dictamen = new DictamenTecnico(request.RequestId, request.Result, request.TechnicalReason, request.Observations, providerId.ToString());
        await _dictamenes.AddAsync(dictamen, cancellationToken);
        assignedCase.CambiarEstado(EstadoAsignacionProveedor.Dictaminado);
        solicitud.CompletarRevisionProveedor(ProviderReviewCompletedReason(request.Result), providerId.ToString());
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TechnicalReportResultDto(dictamen.Id, dictamen.SolicitudId, dictamen.Resultado, dictamen.MotivoTecnico, dictamen.Observaciones, dictamen.IssuedAt);
    }

    public async Task<bool> CanProceedWithProviderDecisionAsync(ProviderDecisionDto request, CancellationToken cancellationToken = default)
    {
        if (request.TechnicalResult != ResultadoDictamen.Procede)
        {
            return false;
        }

        if (request.PreferredSolution is PreferenciaSolucion.Cambio or PreferenciaSolucion.Reparacion)
        {
            var solicitud = await _cases.GetRequestAsync(request.RequestId, cancellationToken)
                ?? throw new BusinessRuleException("Solicitud no encontrada.");

            return await _inventory.HasReplacementStockAsync(solicitud.ProductoId, solicitud.Cantidad, cancellationToken);
        }

        return true;
    }

    private async Task<CasoAsignado> GetAssignedCaseOrThrow(Guid providerId, Guid requestId, CancellationToken cancellationToken)
    {
        return await _cases.GetAssignedCaseAsync(providerId, requestId, cancellationToken)
            ?? throw new BusinessRuleException("El proveedor no esta asignado a este caso.");
    }

    private static ProviderCaseDto ToDto(CasoAsignado assignedCase) =>
        new(assignedCase.Id, assignedCase.SolicitudId, assignedCase.ProveedorId, assignedCase.Estado, assignedCase.AssignedAt);

    private static void EnsureProviderCanWork(Solicitud solicitud, CasoAsignado assignedCase)
    {
        if (solicitud.EstadoActual is not (EstadoSolicitudEnum.EnRevisionProveedor or EstadoSolicitudEnum.EnRevision))
        {
            throw new BusinessRuleException("El caso no esta en revision del proveedor.");
        }

        if (assignedCase.Estado == EstadoAsignacionProveedor.Cerrado)
        {
            throw new BusinessRuleException("El caso asignado ya fue cerrado.");
        }
    }

    private static string ProviderReviewCompletedReason(ResultadoDictamen result) =>
        result switch
        {
            ResultadoDictamen.Procede => "Proveedor dio visto bueno; solicitud pendiente de decision final del administrador.",
            ResultadoDictamen.NoProcede => "Proveedor no dio visto bueno; solicitud pendiente de decision final del administrador.",
            _ => "Proveedor solicita revision adicional; solicitud pendiente de decision final del administrador."
        };
}
