using DevolucionesGarantias.Application.Common.DTOs;

namespace DevolucionesGarantias.Application.Common.Responses;

public sealed record ApplicationResponse<T>(
    bool Succeeded,
    T? Data,
    IReadOnlyCollection<ErrorDto> Errors,
    string? Message = null)
{
    public static ApplicationResponse<T> Success(T data, string? message = null) => new(true, data, [], message);

    public static ApplicationResponse<T> Failure(IEnumerable<ErrorDto> errors, string? message = null) =>
        new(false, default, errors.ToArray(), message);
}
