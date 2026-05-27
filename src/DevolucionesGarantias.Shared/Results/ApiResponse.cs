namespace DevolucionesGarantias.Shared.Results;

public sealed record ApiResponse<T>(
    bool Succeeded,
    T? Data,
    string? Message = null,
    string? TraceId = null)
{
    public static ApiResponse<T> Ok(T data, string? message = null) => new(true, data, message);

    public static ApiResponse<T> Fail(string message, string? traceId = null) => new(false, default, message, traceId);
}
