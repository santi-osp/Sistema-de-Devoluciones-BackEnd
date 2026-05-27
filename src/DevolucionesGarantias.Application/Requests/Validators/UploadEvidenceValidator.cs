using DevolucionesGarantias.Application.Requests.DTOs;
using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Application.Requests.Validators;

public sealed class UploadEvidenceValidator
{
    private const long MaxFileSizeBytes = 25 * 1024 * 1024;

    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/webp",
        "image/heic",
        "image/heif",
        "application/pdf",
        "video/mp4",
        "video/quicktime",
        "video/webm"
    };

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp",
        ".heic",
        ".heif",
        ".pdf",
        ".mp4",
        ".mov",
        ".webm"
    };

    public IReadOnlyCollection<string> Validate(UploadEvidenceDto request)
    {
        var errors = new List<string>();

        if (request.RequestId == Guid.Empty)
        {
            errors.Add("La solicitud es obligatoria.");
        }

        if (string.IsNullOrWhiteSpace(request.FileName))
        {
            errors.Add("El nombre del archivo es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(request.ContentType))
        {
            errors.Add("El tipo de contenido es obligatorio.");
        }

        if (request.Content is null)
        {
            errors.Add("El contenido del archivo es obligatorio.");
        }
        else
        {
            if (request.Content.CanSeek && request.Content.Length <= 0)
            {
                errors.Add("El archivo no puede estar vacio.");
            }

            if (request.Content.CanSeek && request.Content.Length > MaxFileSizeBytes)
            {
                errors.Add("El archivo supera el tamano maximo permitido.");
            }
        }

        if (!string.IsNullOrWhiteSpace(request.ContentType) && !AllowedContentTypes.Contains(request.ContentType))
        {
            errors.Add("El tipo de contenido no esta permitido.");
        }

        if (!string.IsNullOrWhiteSpace(request.FileName))
        {
            var extension = Path.GetExtension(request.FileName);
            if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
            {
                errors.Add("La extension del archivo no esta permitida.");
            }
        }

        return errors;
    }

    public void ValidateAndThrow(UploadEvidenceDto request)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            throw new BusinessRuleException(string.Join(" ", errors));
        }
    }
}
