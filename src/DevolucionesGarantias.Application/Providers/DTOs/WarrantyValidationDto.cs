namespace DevolucionesGarantias.Application.Providers.DTOs;

public sealed record WarrantyValidationDto(Guid RequestId, Guid ProductId, bool IsWarrantyValid, string? Reason = null);
