using DevolucionesGarantias.Application.Common.Security;
using DevolucionesGarantias.Application.Operation.Services;
using DevolucionesGarantias.Application.Providers.Services;
using DevolucionesGarantias.Application.Requests.DTOs;
using DevolucionesGarantias.Application.Requests.Services;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevolucionesGarantias.Api.Controllers;

[Route("api/requests")]
[Authorize(Roles = $"{RoleNames.Cliente},{RoleNames.Administrador},{RoleNames.Analista},{RoleNames.Proveedor}")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "Requests")]
public sealed class RequestsController : ApiControllerBase
{
    private readonly RequestsService _requestsService;
    private readonly EvidenceService _evidenceService;
    private readonly BandejaOperativaService _operationService;
    private readonly GestionProveedorService _providerService;

    public RequestsController(
        RequestsService requestsService,
        EvidenceService evidenceService,
        BandejaOperativaService operationService,
        GestionProveedorService providerService)
    {
        _requestsService = requestsService;
        _evidenceService = evidenceService;
        _operationService = operationService;
        _providerService = providerService;
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Cliente)]
    public async Task<IActionResult> Create(CreateRequestDto request, CancellationToken cancellationToken)
    {
        var created = await _requestsService.CreateAsync(request with { CustomerId = CurrentUserId }, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, ApiResponse<RequestDetailDto>.Ok(created));
    }

    [HttpGet]
    [Authorize(Roles = RoleNames.Cliente)]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<RequestSummaryDto>>>> List(CancellationToken cancellationToken)
    {
        var requests = await _requestsService.ListByCustomerAsync(CurrentUserId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<RequestSummaryDto>>.Ok(requests));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        if (IsBackOffice)
        {
            return Ok(ApiResponse<object>.Ok(await _operationService.GetDetailAsync(id, cancellationToken)));
        }

        if (IsProveedor)
        {
            var detail = await _providerService.GetAssignedCaseAsync(CurrentUserId, id, cancellationToken);
            return Ok(ApiResponse<RequestDetailDto>.Ok(detail.Request));
        }

        var request = await _requestsService.GetDetailAsync(id, CurrentUserId, cancellationToken);
        return request is null ? NotFound(ApiResponse<object>.Fail("Solicitud no encontrada.")) : Ok(ApiResponse<RequestDetailDto>.Ok(request));
    }

    [HttpGet("{id:guid}/timeline")]
    public async Task<IActionResult> Timeline(Guid id, CancellationToken cancellationToken)
    {
        var detail = await GetRequestDetailForCurrentActor(id, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<RequestTimelineDto>>.Ok(detail.Timeline));
    }

    [HttpPost("{id:guid}/evidence")]
    [Authorize(Roles = RoleNames.Cliente)]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<EvidenceDto>>> UploadEvidence(
        Guid id,
        [FromForm] UploadEvidenceForm form,
        CancellationToken cancellationToken)
    {
        await using var stream = form.File.OpenReadStream();
        var uploaded = await _evidenceService.UploadAsync(
            new UploadEvidenceDto(id, form.Type, form.File.FileName, form.File.ContentType, stream),
            CurrentUserId,
            cancellationToken);

        return Ok(ApiResponse<EvidenceDto>.Ok(uploaded));
    }

    [HttpGet("{id:guid}/evidence")]
    public async Task<IActionResult> Evidence(Guid id, CancellationToken cancellationToken)
    {
        if (!IsCliente)
        {
            var detail = await GetRequestDetailForCurrentActor(id, cancellationToken);
            return Ok(ApiResponse<IReadOnlyCollection<EvidenceDto>>.Ok(detail.Evidence));
        }

        var evidence = await _evidenceService.ListByRequestAsync(id, CurrentUserId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<EvidenceDto>>.Ok(evidence));
    }

    private async Task<RequestDetailDto> GetRequestDetailForCurrentActor(Guid id, CancellationToken cancellationToken)
    {
        if (IsProveedor)
        {
            return (await _providerService.GetAssignedCaseAsync(CurrentUserId, id, cancellationToken)).Request;
        }

        if (IsBackOffice)
        {
            var operationDetail = await _operationService.GetDetailAsync(id, cancellationToken);
            return new RequestDetailDto(
                operationDetail.Id,
                operationDetail.CustomerId,
                operationDetail.OrderId,
                operationDetail.ProductId,
                operationDetail.Type,
                operationDetail.Reason,
                operationDetail.Description,
                operationDetail.Quantity,
                PreferenciaSolucion.Cambio,
                operationDetail.Status,
                DateTimeOffset.MinValue,
                operationDetail.Evidence,
                []);
        }

        return await _requestsService.GetDetailAsync(id, CurrentUserId, cancellationToken)
            ?? throw new KeyNotFoundException("Solicitud no encontrada.");
    }

    public sealed class UploadEvidenceForm
    {
        public TipoEvidencia Type { get; init; }
        public IFormFile File { get; init; } = null!;
    }
}
