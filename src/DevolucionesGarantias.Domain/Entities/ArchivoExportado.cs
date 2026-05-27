using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;
using DevolucionesGarantias.Domain.ValueObjects;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class ArchivoExportado : IEntity
{
    public ArchivoExportado(Guid reporteId, FormatoReporte formato, FilePath archivo, string nombreArchivo, string exportedBy)
    {
        if (reporteId == Guid.Empty)
        {
            throw new BusinessRuleException("El archivo exportado debe asociarse a un reporte.");
        }

        if (string.IsNullOrWhiteSpace(nombreArchivo))
        {
            throw new BusinessRuleException("El nombre del archivo exportado es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(exportedBy))
        {
            throw new BusinessRuleException("Debe registrarse quien exporta el reporte.");
        }

        Id = Guid.NewGuid();
        ReporteId = reporteId;
        Formato = formato;
        Archivo = archivo;
        NombreArchivo = nombreArchivo.Trim();
        ExportedBy = exportedBy.Trim();
        ExportedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid ReporteId { get; private set; }
    public FormatoReporte Formato { get; private set; }
    public FilePath Archivo { get; private set; }
    public string NombreArchivo { get; private set; }
    public string ExportedBy { get; private set; }
    public DateTimeOffset ExportedAt { get; private set; }
}

