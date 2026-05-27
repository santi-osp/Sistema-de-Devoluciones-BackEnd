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
    private readonly IUnitOfWork _unitOfWork;
    private readonly WarrantyValidationValidator _warrantyValidator;
    private readonly TechnicalReportValidator _technicalReportValidator;

    public GestionProveedorService(
        IProviderCaseRepository cases,
        IDictamenRepository dictamenes,
        IInventarioService inventory,
        IUnitOfWork unitOfWork,
        WarrantyValidationValidator warrantyValidator,
        TechnicalReportValidator technicalReportValidator)
    {
        _cases = cases;
        _dictamenes = dictamenes;
        _inventory = inventory;
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

        return new ProviderCaseDetailDto(assignedCase.Id, assignedCase.SolicitudId, assignedCase.ProveedorId, assignedCase.Estado, request.ToDetailDto());
    }

    public async Task<WarrantyValidationResultDto> ValidateWarrantyAsync(Guid providerId, WarrantyValidationDto request, CancellationToken cancellationToken = default)
    {
        _warrantyValidator.ValidateAndThrow(request);
        await GetAssignedCaseOrThrow(providerId, request.RequestId, cancellationToken);

        var validation = new ValidacionGarantia(request.RequestId, request.ProductId, request.IsWarrantyValid, providerId.ToString(), request.Reason);
        await _cases.AddWarrantyValidationAsync(validation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new WarrantyValidationResultDto(validation.Id, validation.SolicitudId, validation.ProductoId, validation.IsWarrantyValid, validation.Reason, validation.ValidatedAt);
    }

    public async Task<TechnicalReportResultDto> RegisterTechnicalReportAsync(Guid providerId, TechnicalReportDto request, CancellationToken cancellationToken = default)
    {
        _technicalReportValidator.ValidateAndThrow(request);
        await GetAssignedCaseOrThrow(providerId, request.RequestId, cancellationToken);

        var dictamen = new DictamenTecnico(request.RequestId, request.Result, request.TechnicalReason, request.Observations, providerId.ToString());
        await _dictamenes.AddAsync(dictamen, cancellationToken);
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
            return await _inventory.HasReplacementStockAsync(request.RequestId, 1, cancellationToken);
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
}
