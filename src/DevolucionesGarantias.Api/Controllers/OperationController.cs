using DevolucionesGarantias.Application.Common.Security;
using DevolucionesGarantias.Application.Operation.DTOs;
using DevolucionesGarantias.Application.Operation.Services;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevolucionesGarantias.Api.Controllers;

[Route("api/operation")]
[Authorize(Roles = $"{RoleNames.Administrador},{RoleNames.Analista}")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "Operation")]
public sealed class OperationController : ApiControllerBase
{
    private readonly BandejaOperativaService _bandeja;
    private readonly RevisionOperativaService _revision;

    public OperationController(BandejaOperativaService bandeja, RevisionOperativaService revision)
    {
        _bandeja = bandeja;
        _revision = revision;
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<ApiResponse<OperationDashboardDto>>> Dashboard(CancellationToken cancellationToken)
    {
        var dashboard = await _bandeja.GetDashboardAsync(cancellationToken);
        return Ok(ApiResponse<OperationDashboardDto>.Ok(dashboard));
    }

    [HttpGet("requests")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<OperationRequestDto>>>> Requests([FromQuery] EstadoSolicitudEnum? status, CancellationToken cancellationToken)
    {
        var requests = await _bandeja.ListAsync(status, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<OperationRequestDto>>.Ok(requests));
    }

    [HttpGet("requests/{id:guid}")]
    public async Task<ActionResult<ApiResponse<OperationRequestDetailDto>>> RequestDetail(Guid id, CancellationToken cancellationToken)
    {
        var detail = await _bandeja.GetDetailAsync(id, cancellationToken);
        return Ok(ApiResponse<OperationRequestDetailDto>.Ok(detail));
    }

    [HttpPost("requests/{id:guid}/comments")]
    public async Task<ActionResult<ApiResponse<InternalCommentDto>>> AddComment(Guid id, CreateCommentDto request, CancellationToken cancellationToken)
    {
        var comment = await _revision.AddCommentAsync(request with { RequestId = id }, CurrentUserId, cancellationToken);
        return Ok(ApiResponse<InternalCommentDto>.Ok(comment));
    }

    [HttpPost("requests/{id:guid}/request-information")]
    public async Task<IActionResult> RequestInformation(Guid id, RequestInformationDto request, CancellationToken cancellationToken)
    {
        await _revision.RequestInformationAsync(request with { RequestId = id }, CurrentUserId, cancellationToken);
        return Ok(ApiResponse<object>.Ok(new { requested = true }));
    }

    [HttpPost("requests/{id:guid}/approve")]
    public async Task<ActionResult<ApiResponse<DecisionResultDto>>> Approve(Guid id, DecisionRequest request, CancellationToken cancellationToken)
    {
        var result = await _revision.DecideAsync(new DecisionDto(id, true, request.Reason), CurrentUserId, cancellationToken);
        return Ok(ApiResponse<DecisionResultDto>.Ok(result));
    }

    [HttpPost("requests/{id:guid}/reject")]
    public async Task<ActionResult<ApiResponse<DecisionResultDto>>> Reject(Guid id, DecisionRequest request, CancellationToken cancellationToken)
    {
        var result = await _revision.DecideAsync(new DecisionDto(id, false, request.Reason), CurrentUserId, cancellationToken);
        return Ok(ApiResponse<DecisionResultDto>.Ok(result));
    }
}

public sealed record DecisionRequest(string Reason);
