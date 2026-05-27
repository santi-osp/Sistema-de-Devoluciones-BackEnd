using DevolucionesGarantias.Application.Common.Security;
using DevolucionesGarantias.Application.Providers.DTOs;
using DevolucionesGarantias.Application.Providers.Services;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevolucionesGarantias.Api.Controllers;

[Route("api/providers")]
[Authorize(Roles = RoleNames.Proveedor)]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "Providers")]
public sealed class ProvidersController : ApiControllerBase
{
    private readonly GestionProveedorService _providerService;
    private readonly RecepcionProductoService _receptionService;

    public ProvidersController(GestionProveedorService providerService, RecepcionProductoService receptionService)
    {
        _providerService = providerService;
        _receptionService = receptionService;
    }

    [HttpGet("cases")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<ProviderCaseDto>>>> Cases(CancellationToken cancellationToken)
    {
        var cases = await _providerService.ListAssignedAsync(CurrentUserId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<ProviderCaseDto>>.Ok(cases));
    }

    [HttpGet("cases/{id:guid}")]
    public async Task<ActionResult<ApiResponse<ProviderCaseDetailDto>>> Case(Guid id, CancellationToken cancellationToken)
    {
        var detail = await _providerService.GetAssignedCaseAsync(CurrentUserId, id, cancellationToken);
        return Ok(ApiResponse<ProviderCaseDetailDto>.Ok(detail));
    }

    [HttpPost("cases/{id:guid}/warranty-validation")]
    public async Task<ActionResult<ApiResponse<WarrantyValidationResultDto>>> ValidateWarranty(
        Guid id,
        WarrantyValidationDto request,
        CancellationToken cancellationToken)
    {
        var result = await _providerService.ValidateWarrantyAsync(CurrentUserId, request with { RequestId = id }, cancellationToken);
        return Ok(ApiResponse<WarrantyValidationResultDto>.Ok(result));
    }

    [HttpPost("cases/{id:guid}/technical-report")]
    public async Task<ActionResult<ApiResponse<TechnicalReportResultDto>>> TechnicalReport(
        Guid id,
        TechnicalReportDto request,
        CancellationToken cancellationToken)
    {
        var result = await _providerService.RegisterTechnicalReportAsync(CurrentUserId, request with { RequestId = id }, cancellationToken);
        return Ok(ApiResponse<TechnicalReportResultDto>.Ok(result));
    }

    [HttpPost("cases/{id:guid}/authorize-repair")]
    public async Task<ActionResult<ApiResponse<object>>> AuthorizeRepair(Guid id, CancellationToken cancellationToken)
    {
        var allowed = await _providerService.CanProceedWithProviderDecisionAsync(
            new ProviderDecisionDto(id, PreferenciaSolucion.Reparacion, ResultadoDictamen.Procede),
            cancellationToken);

        return Ok(ApiResponse<object>.Ok(new { authorized = allowed }));
    }

    [HttpPost("cases/{id:guid}/authorize-replacement")]
    public async Task<ActionResult<ApiResponse<object>>> AuthorizeReplacement(Guid id, CancellationToken cancellationToken)
    {
        var allowed = await _providerService.CanProceedWithProviderDecisionAsync(
            new ProviderDecisionDto(id, PreferenciaSolucion.Cambio, ResultadoDictamen.Procede),
            cancellationToken);

        return Ok(ApiResponse<object>.Ok(new { authorized = allowed }));
    }

    [HttpPost("cases/{id:guid}/reception")]
    public async Task<ActionResult<ApiResponse<ProductReceptionResultDto>>> Reception(
        Guid id,
        ProductReceptionDto request,
        CancellationToken cancellationToken)
    {
        var result = await _receptionService.RegisterReceptionAsync(CurrentUserId, request with { RequestId = id }, cancellationToken);
        return Ok(ApiResponse<ProductReceptionResultDto>.Ok(result));
    }
}
