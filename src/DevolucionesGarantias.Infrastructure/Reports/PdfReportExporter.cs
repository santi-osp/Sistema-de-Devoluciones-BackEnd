using System.Text;
using DevolucionesGarantias.Application.Reports.Interfaces;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Infrastructure.Reports;

public sealed class PdfReportExporter : IExportadorReporte
{
    public FormatoReporte Format => FormatoReporte.Pdf;

    public Task<GeneratedReportFile> ExportAsync(Reporte report, CancellationToken cancellationToken = default)
    {
        var fileName = $"report-{report.Id:N}.pdf";
        var content = BuildPdfContent(report);
        return Task.FromResult(new GeneratedReportFile(Format, fileName, "application/pdf", content));
    }

    private static byte[] BuildPdfContent(Reporte report)
    {
        var lines = new List<string>
        {
            $"Reporte: {report.Titulo}",
            $"Generado: {report.GeneratedAt:yyyy-MM-dd HH:mm:ss} UTC",
            string.Empty,
            "Metricas:"
        };

        lines.AddRange(report.Metricas.Select(metric => $"{metric.Nombre}: {metric.Valor} {metric.Unidad}"));

        var textCommands = new StringBuilder();
        textCommands.AppendLine("BT");
        textCommands.AppendLine("/F1 11 Tf");
        textCommands.AppendLine("50 790 Td");

        foreach (var line in lines)
        {
            textCommands.AppendLine($"({EscapePdfText(line)}) Tj");
            textCommands.AppendLine("0 -18 Td");
        }

        textCommands.AppendLine("ET");

        var streamContent = textCommands.ToString();
        var objects = new[]
        {
            "<< /Type /Catalog /Pages 2 0 R >>",
            "<< /Type /Pages /Kids [3 0 R] /Count 1 >>",
            "<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 842] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>",
            "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>",
            $"<< /Length {Encoding.ASCII.GetByteCount(streamContent)} >>\nstream\n{streamContent}endstream"
        };

        using var memory = new MemoryStream();
        using var writer = new StreamWriter(memory, Encoding.ASCII, leaveOpen: true);

        writer.WriteLine("%PDF-1.4");
        var offsets = new List<long> { 0 };
        for (var index = 0; index < objects.Length; index++)
        {
            writer.Flush();
            offsets.Add(memory.Position);
            writer.WriteLine($"{index + 1} 0 obj");
            writer.WriteLine(objects[index]);
            writer.WriteLine("endobj");
        }

        writer.Flush();
        var xrefOffset = memory.Position;
        writer.WriteLine("xref");
        writer.WriteLine($"0 {objects.Length + 1}");
        writer.WriteLine("0000000000 65535 f ");
        foreach (var offset in offsets.Skip(1))
        {
            writer.WriteLine($"{offset:0000000000} 00000 n ");
        }

        writer.WriteLine("trailer");
        writer.WriteLine($"<< /Size {objects.Length + 1} /Root 1 0 R >>");
        writer.WriteLine("startxref");
        writer.WriteLine(xrefOffset);
        writer.WriteLine("%%EOF");
        writer.Flush();

        return memory.ToArray();
    }

    private static string EscapePdfText(string value) =>
        value
            .Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("(", "\\(", StringComparison.Ordinal)
            .Replace(")", "\\)", StringComparison.Ordinal);
}
