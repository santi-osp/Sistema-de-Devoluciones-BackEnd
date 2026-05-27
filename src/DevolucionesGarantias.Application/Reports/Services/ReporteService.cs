using DevolucionesGarantias.Application.Common.Interfaces;
using DevolucionesGarantias.Application.Reports.DTOs;
using DevolucionesGarantias.Application.Reports.Interfaces;
using DevolucionesGarantias.Application.Reports.Validators;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.ValueObjects;

namespace DevolucionesGarantias.Application.Reports.Services;

public sealed class ReporteService
{
    private const string DefaultReportsBucket = "report-files";

    private readonly IConsultaReportesRepository _reports;
    private readonly IEnumerable<IExportadorReporte> _exporters;
    private readonly IFileStorageService _storage;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ReportFilterValidator _validator;

    public ReporteService(
        IConsultaReportesRepository reports,
        IEnumerable<IExportadorReporte> exporters,
        IFileStorageService storage,
        IUnitOfWork unitOfWork,
        ReportFilterValidator validator)
    {
        _reports = reports;
        _exporters = exporters;
        _storage = storage;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<ReportDto> GenerateAsync(string title, ReportFilterDto filterDto, Guid generatedBy, CancellationToken cancellationToken = default)
    {
        _validator.ValidateAndThrow(filterDto);

        var filter = ToDomainFilter(filterDto);
        var report = new Reporte(title, filter, generatedBy.ToString());
        var metrics = await _reports.CalculateMetricsAsync(filter, report.Id, cancellationToken);

        foreach (var metric in metrics)
        {
            report.AgregarMetrica(metric);
        }

        await _reports.AddAsync(report, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ToDto(report);
    }

    public async Task<ReportDto?> GetAsync(Guid reportId, CancellationToken cancellationToken = default)
    {
        var report = await _reports.GetByIdAsync(reportId, cancellationToken);
        return report is null ? null : ToDto(report);
    }

    public async Task<IReadOnlyCollection<ReportSummaryDto>> ListAsync(CancellationToken cancellationToken = default)
    {
        var reports = await _reports.ListAsync(cancellationToken);
        return reports
            .Select(report => new ReportSummaryDto(report.Id, report.Titulo, report.GeneratedBy, report.GeneratedAt, report.Metricas.Count))
            .ToArray();
    }

    public async Task<ReportExportFileDto> ExportAsync(ExportReportRequestDto request, Guid exportedBy, CancellationToken cancellationToken = default)
    {
        var report = await _reports.GetByIdAsync(request.ReportId, cancellationToken)
            ?? throw new BusinessRuleException("Reporte no encontrado.");
        var exporter = _exporters.FirstOrDefault(item => item.Format == request.Format)
            ?? throw new BusinessRuleException($"No existe exportador para formato {request.Format}.");

        var generatedFile = await exporter.ExportAsync(report, cancellationToken);
        await using var stream = new MemoryStream(generatedFile.Content, writable: false);
        var storedFile = await _storage.StoreAsync(
            DefaultReportsBucket,
            generatedFile.FileName,
            stream,
            generatedFile.ContentType,
            $"reports/{report.Id:D}/{request.Format.ToString().ToLowerInvariant()}",
            cancellationToken);

        var file = new ArchivoExportado(
            report.Id,
            request.Format,
            new FilePath(storedFile.Bucket, storedFile.Path, storedFile.Url),
            storedFile.OriginalFileName,
            exportedBy.ToString());

        await _reports.AddExportedFileAsync(file, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ReportExportFileDto(report.Id, request.Format, generatedFile.FileName, generatedFile.ContentType, generatedFile.Content, ToDto(file));
    }

    public async Task<IReadOnlyCollection<MetricDto>> CalculateMetricsAsync(ReportFilterDto filterDto, CancellationToken cancellationToken = default)
    {
        _validator.ValidateAndThrow(filterDto);
        var metrics = await _reports.CalculateMetricsAsync(ToDomainFilter(filterDto), Guid.NewGuid(), cancellationToken);
        return metrics.Select(metric => new MetricDto(metric.Nombre, metric.Valor, metric.Unidad)).ToArray();
    }

    private static FiltroReporte ToDomainFilter(ReportFilterDto filter)
    {
        DateRange? range = null;
        if (filter.StartDate.HasValue && filter.EndDate.HasValue)
        {
            range = new DateRange(filter.StartDate.Value, filter.EndDate.Value);
        }

        return new FiltroReporte(range, filter.Status, filter.RequestType);
    }

    private static ReportDto ToDto(Reporte report) =>
        new(
            report.Id,
            report.Titulo,
            new ReportFilterDto(report.Filtro.Periodo?.Start, report.Filtro.Periodo?.End, report.Filtro.Estado, report.Filtro.TipoSolicitud),
            report.GeneratedBy,
            report.GeneratedAt,
            report.Metricas.Select(metric => new MetricDto(metric.Nombre, metric.Valor, metric.Unidad)).ToArray(),
            report.Archivos.Select(ToDto).ToArray());

    private static ExportedFileDto ToDto(ArchivoExportado file) =>
        new(
            file.Id,
            file.ReporteId,
            file.Formato,
            file.Archivo.Bucket,
            file.Archivo.Path,
            file.Archivo.Url,
            file.NombreArchivo,
            file.ExportedAt);
}
