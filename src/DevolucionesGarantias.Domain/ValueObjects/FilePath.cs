using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Domain.ValueObjects;

public sealed record FilePath
{
    public string Bucket { get; }
    public string Path { get; }
    public string? Url { get; }

    public FilePath(string bucket, string path, string? url = null)
    {
        if (string.IsNullOrWhiteSpace(bucket))
        {
            throw new BusinessRuleException("El bucket del archivo es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(path))
        {
            throw new BusinessRuleException("La ruta del archivo es obligatoria.");
        }

        Bucket = bucket.Trim();
        Path = path.Trim();
        Url = string.IsNullOrWhiteSpace(url) ? null : url.Trim();
    }

    public override string ToString() => Url ?? $"{Bucket}/{Path}";
}

