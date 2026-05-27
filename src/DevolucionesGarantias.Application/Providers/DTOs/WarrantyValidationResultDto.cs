namespace DevolucionesGarantias.Application.Providers.DTOs;

public sealed record WarrantyValidationResultDto(
    Guid Id,
    Guid RequestId,
    Guid ProductId,
    bool IsWarrantyValid,
    string? Reason,
    DateTimeOffset ValidatedAt);
