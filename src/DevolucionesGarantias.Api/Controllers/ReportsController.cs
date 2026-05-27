using DevolucionesGarantias.Application.Common.Security;
using DevolucionesGarantias.Application.Reports.DTOs;
using DevolucionesGarantias.Application.Reports.Services;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevolucionesGarantias.Api.Controllers;

[Route("api/reports")]
[Authorize(Roles = $"{RoleNames.Administrador},{RoleNames.Analista}")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "Reports")]
public sealed class ReportsController : ApiControllerBase
{
    private readonly ReporteService _reports;

    public ReportsController(ReporteService reports)
    {
        _reports = reports;
    }

    [HttpPost("generate")]
    public async Task<ActionResult<ApiResponse<ReportDto>>> Generate(GenerateReportRequest request, CancellationToken cancellationToken)
    {
        var report = await _reports.GenerateAsync(request.Title, request.Filter, CurrentUserId, cancellationToken);
        return Ok(ApiResponse<ReportDto>.Ok(report));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<ReportSummaryDto>>>> List(CancellationToken cancellationToken)
    {
        var reports = await _reports.ListAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<ReportSummaryDto>>.Ok(reports));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<ReportDto>>> Get(Guid id, CancellationToken cancellationToken)
    {
        var report = await _reports.GetAsync(id, cancellationToken);
        return report is null ? NotFound(ApiResponse<object>.Fail("Reporte no encontrado.")) : Ok(ApiResponse<ReportDto>.Ok(report));
    }

    [HttpGet("metrics")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<MetricDto>>>> Metrics(
        [FromQuery] DateTimeOffset? startDate,
        [FromQuery] DateTimeOffset? endDate,
        [FromQuery] EstadoSolicitudEnum? status,
        [FromQuery] TipoSolicitud? requestType,
        CancellationToken cancellationToken)
    {
        var metrics = await _reports.CalculateMetricsAsync(new ReportFilterDto(startDate, endDate, status, requestType), cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<MetricDto>>.Ok(metrics));
    }

    [HttpGet("{id:guid}/export")]
    public async Task<IActionResult> Export(Guid id, [FromQuery] FormatoReporte format, CancellationToken cancellationToken)
    {
        var file = await _reports.ExportAsync(new ExportReportRequestDto(id, format), CurrentUserId, cancellationToken);
        return File(file.Content, file.ContentType, file.FileName);
    }
}

public sealed record GenerateReportRequest(string Title, ReportFilterDto Filter);
