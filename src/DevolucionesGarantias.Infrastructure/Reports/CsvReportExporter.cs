using System.Text;
using DevolucionesGarantias.Application.Reports.Interfaces;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Infrastructure.Reports;

public sealed class CsvReportExporter : IExportadorReporte
{
    public FormatoReporte Format => FormatoReporte.Csv;

    public Task<GeneratedReportFile> ExportAsync(Reporte report, CancellationToken cancellationToken = default)
    {
        var fileName = $"report-{report.Id:N}.csv";
        var content = BuildContent(report);
        return Task.FromResult(new GeneratedReportFile(Format, fileName, "text/csv", content));
    }

    private static byte[] BuildContent(Reporte report)
    {
        var builder = new StringBuilder();
        builder.AppendLine("metric,value,unit");
        foreach (var metric in report.Metricas)
        {
            builder.AppendLine($"{Escape(metric.Nombre)},{metric.Valor},{Escape(metric.Unidad)}");
        }

        return Encoding.UTF8.GetBytes(builder.ToString());
    }

    private static string Escape(string value) => $"\"{value.Replace("\"", "\"\"", StringComparison.Ordinal)}\"";
}
