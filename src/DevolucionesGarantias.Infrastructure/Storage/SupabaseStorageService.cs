using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using DevolucionesGarantias.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace DevolucionesGarantias.Infrastructure.Storage;

public sealed class SupabaseStorageService : IFileStorageService
{
    private static readonly Regex UnsafeFileNameCharacters = new("[^a-zA-Z0-9._-]+", RegexOptions.Compiled);

    private readonly HttpClient _httpClient;
    private readonly SupabaseStorageOptions _storageOptions;
    private readonly FileValidationOptions _fileOptions;

    public SupabaseStorageService(
        HttpClient httpClient,
        IOptions<SupabaseStorageOptions> storageOptions,
        IOptions<FileValidationOptions> fileOptions)
    {
        _httpClient = httpClient;
        _storageOptions = storageOptions.Value;
        _fileOptions = fileOptions.Value;
    }

    public async Task<StoredFileDescriptor> StoreAsync(
        string bucket,
        string fileName,
        Stream content,
        string contentType,
        string pathPrefix,
        CancellationToken cancellationToken = default)
    {
        ValidateConfiguration();
        ValidateFile(fileName, content, contentType);

        var resolvedBucket = ResolveBucket(bucket);
        var path = GeneratePath(pathPrefix, fileName);
        using var request = new HttpRequestMessage(HttpMethod.Post, $"storage/v1/object/{resolvedBucket}/{path}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _storageOptions.ServiceRoleKey);
        request.Headers.Add("apikey", _storageOptions.ServiceRoleKey);
        request.Content = new StreamContent(content);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"No se pudo guardar el archivo en Supabase Storage. StatusCode: {(int)response.StatusCode}.");
        }

        return new StoredFileDescriptor(
            resolvedBucket,
            path,
            BuildPublicUrl(resolvedBucket, path),
            SafeFileName(fileName),
            content.CanSeek ? content.Length : 0,
            contentType);
    }

    public string GeneratePath(string pathPrefix, string fileName)
    {
        var safePrefix = string.Join(
            "/",
            pathPrefix
                .Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(SafePathSegment));
        var safeFileName = SafeFileName(fileName);
        return $"{safePrefix}/{Guid.NewGuid():N}-{safeFileName}";
    }

    private string ResolveBucket(string bucket)
    {
        if (!string.IsNullOrWhiteSpace(bucket))
        {
            return bucket.Trim();
        }

        return _storageOptions.EvidenceBucket;
    }

    private string BuildPublicUrl(string bucket, string path) =>
        $"{_storageOptions.Url.TrimEnd('/')}/storage/v1/object/public/{bucket}/{path}";

    private void ValidateConfiguration()
    {
        if (string.IsNullOrWhiteSpace(_storageOptions.Url) || _storageOptions.Url.Contains("YOUR_", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Supabase:Url no esta configurado.");
        }

        if (string.IsNullOrWhiteSpace(_storageOptions.ServiceRoleKey) || _storageOptions.ServiceRoleKey.Contains("YOUR_", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Supabase:ServiceRoleKey no esta configurado.");
        }

        _httpClient.BaseAddress ??= new Uri(_storageOptions.Url.TrimEnd('/') + "/");
    }

    private void ValidateFile(string fileName, Stream content, string contentType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);
        ArgumentNullException.ThrowIfNull(content);

        if (content.CanSeek && content.Length <= 0)
        {
            throw new InvalidOperationException("El archivo no puede estar vacio.");
        }

        if (content.CanSeek && content.Length > _fileOptions.MaxFileSizeBytes)
        {
            throw new InvalidOperationException("El archivo supera el tamano maximo permitido.");
        }

        if (_fileOptions.AllowedContentTypes.Length > 0 &&
            !_fileOptions.AllowedContentTypes.Any(allowed => allowed.Equals(contentType, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("El tipo de archivo no esta permitido.");
        }

        var extension = Path.GetExtension(fileName);
        if (string.IsNullOrWhiteSpace(extension))
        {
            throw new InvalidOperationException("La extension del archivo es obligatoria.");
        }
    }

    private static string SafeFileName(string fileName)
    {
        var onlyFileName = Path.GetFileName(fileName.Trim());
        var safeFileName = UnsafeFileNameCharacters.Replace(onlyFileName, "-").Trim('-', '.');
        return string.IsNullOrWhiteSpace(safeFileName) ? "file" : safeFileName;
    }

    private static string SafePathSegment(string segment)
    {
        var safeSegment = UnsafeFileNameCharacters.Replace(segment.Trim(), "-").Trim('-', '.');
        return string.IsNullOrWhiteSpace(safeSegment) ? "segment" : safeSegment;
    }
}
