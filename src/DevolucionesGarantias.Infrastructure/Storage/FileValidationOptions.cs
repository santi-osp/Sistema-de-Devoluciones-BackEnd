namespace DevolucionesGarantias.Infrastructure.Storage;

public sealed class FileValidationOptions
{
    public long MaxFileSizeBytes { get; set; } = 25 * 1024 * 1024;
    public string[] AllowedContentTypes { get; set; } =
    [
        "image/jpeg",
        "image/png",
        "image/webp",
        "image/heic",
        "image/heif",
        "application/pdf",
        "video/mp4",
        "video/quicktime",
        "video/webm",
        "text/csv"
    ];
}
