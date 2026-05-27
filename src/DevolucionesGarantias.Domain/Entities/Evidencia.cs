using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;
using DevolucionesGarantias.Domain.ValueObjects;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class Evidencia : IEntity
{
    public Evidencia(Guid solicitudId, TipoEvidencia tipo, FilePath archivo, string nombreArchivo, long sizeInBytes, string? uploadedBy = null)
    {
        if (solicitudId == Guid.Empty)
        {
            throw new BusinessRuleException("La evidencia debe asociarse a una solicitud.");
        }

        if (string.IsNullOrWhiteSpace(nombreArchivo))
        {
            throw new BusinessRuleException("El nombre del archivo de evidencia es obligatorio.");
        }

        if (sizeInBytes < 0)
        {
            throw new BusinessRuleException("El tamano de la evidencia no puede ser negativo.");
        }

        Id = Guid.NewGuid();
        SolicitudId = solicitudId;
        Tipo = tipo;
        Archivo = archivo;
        NombreArchivo = nombreArchivo.Trim();
        SizeInBytes = sizeInBytes;
        UploadedBy = uploadedBy;
        UploadedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid SolicitudId { get; private set; }
    public TipoEvidencia Tipo { get; private set; }
    public FilePath Archivo { get; private set; }
    public string NombreArchivo { get; private set; }
    public long SizeInBytes { get; private set; }
    public string? UploadedBy { get; private set; }
    public DateTimeOffset UploadedAt { get; private set; }
}

