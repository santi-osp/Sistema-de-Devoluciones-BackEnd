using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Providers.DTOs;

public sealed record ProviderReviewDto(
    ProviderAssignmentDto? Assignment,
    ProviderWarrantyValidationDto? WarrantyValidation,
    ProviderTechnicalReportDto? TechnicalReport,
    ProviderAvailabilityDto Availability);

public sealed record ProviderAssignmentDto(
    Guid Id,
    Guid RequestId,
    Guid ProviderId,
    EstadoAsignacionProveedor Status,
    string AssignedBy,
    DateTimeOffset AssignedAt);

public sealed record ProviderWarrantyValidationDto(
    Guid Id,
    Guid RequestId,
    Guid ProductId,
    bool IsWarrantyValid,
    string? Reason,
    string ValidatedBy,
    DateTimeOffset ValidatedAt);

public sealed record ProviderTechnicalReportDto(
    Guid Id,
    Guid RequestId,
    ResultadoDictamen Result,
    string TechnicalReason,
    string Observations,
    string IssuedBy,
    DateTimeOffset IssuedAt);

public sealed record ProviderAvailabilityDto(
    bool Evaluated,
    PreferenciaSolucion PreferredSolution,
    bool? HasAvailability,
    bool HasConflict,
    string? ConflictReason,
    string? ConflictResolution);
