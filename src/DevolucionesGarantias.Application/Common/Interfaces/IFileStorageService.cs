namespace DevolucionesGarantias.Application.Common.Interfaces;

public interface IFileStorageService
{
    Task<StoredFileDescriptor> StoreAsync(
        string bucket,
        string fileName,
        Stream content,
        string contentType,
        string pathPrefix,
        CancellationToken cancellationToken = default);
}

public sealed record StoredFileDescriptor(
    string Bucket,
    string Path,
    string? Url,
    string OriginalFileName,
    long SizeInBytes,
    string ContentType);
