using DevolucionesGarantias.Application.Providers.DTOs;
using DevolucionesGarantias.Application.Providers.Interfaces;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Providers.Services;

public sealed class ProviderReviewProjectionService
{
    private readonly IInventarioService _inventory;

    public ProviderReviewProjectionService(IInventarioService inventory)
    {
        _inventory = inventory;
    }

    public async Task<ProviderReviewDto> BuildAsync(
        Solicitud request,
        CasoAsignado? assignedCase,
        ValidacionGarantia? warrantyValidation,
        DictamenTecnico? technicalReport,
        CancellationToken cancellationToken = default)
    {
        var availability = await BuildAvailabilityAsync(request, warrantyValidation, technicalReport, cancellationToken);

        return new ProviderReviewDto(
            assignedCase is null ? null : ToAssignmentDto(assignedCase),
            warrantyValidation is null ? null : ToWarrantyDto(warrantyValidation),
            technicalReport is null ? null : ToTechnicalReportDto(technicalReport),
            availability);
    }

    private async Task<ProviderAvailabilityDto> BuildAvailabilityAsync(
        Solicitud request,
        ValidacionGarantia? warrantyValidation,
        DictamenTecnico? technicalReport,
        CancellationToken cancellationToken)
    {
        var conflictReasons = new List<string>();
        bool? hasAvailability = null;

        if (warrantyValidation is not null && !warrantyValidation.IsWarrantyValid)
        {
            conflictReasons.Add(WithDetail("Garantia no vigente", warrantyValidation.Reason));
            hasAvailability = false;
        }

        if (technicalReport is null)
        {
            var hasWarrantyConflict = conflictReasons.Count > 0;
            return new ProviderAvailabilityDto(
                hasWarrantyConflict,
                request.PreferenciaSolucion,
                hasAvailability,
                hasWarrantyConflict,
                JoinReasons(conflictReasons),
                hasWarrantyConflict
                    ? "El administrador debe revisar la validacion de garantia antes de tomar la decision final."
                    : null);
        }

        switch (technicalReport.Resultado)
        {
            case ResultadoDictamen.NoProcede:
                conflictReasons.Add(WithDetail("Proveedor no dio visto bueno", FirstText(technicalReport.MotivoTecnico, technicalReport.Observaciones)));
                hasAvailability = false;
                break;
            case ResultadoDictamen.RequiereRevisionAdicional:
                conflictReasons.Add(WithDetail("Proveedor solicita revision adicional", FirstText(technicalReport.Observaciones, technicalReport.MotivoTecnico)));
                break;
            case ResultadoDictamen.Procede:
                if (request.PreferenciaSolucion is PreferenciaSolucion.Cambio or PreferenciaSolucion.Reparacion)
                {
                    var stockAvailable = await _inventory.HasReplacementStockAsync(request.ProductoId, request.Cantidad, cancellationToken);
                    if (hasAvailability != false)
                    {
                        hasAvailability = stockAvailable;
                    }

                    if (!stockAvailable)
                    {
                        conflictReasons.Add($"No hay disponibilidad para {request.PreferenciaSolucion} del producto solicitado.");
                    }
                }
                else if (hasAvailability != false)
                {
                    hasAvailability = true;
                }

                break;
        }

        var hasConflict = conflictReasons.Count > 0;

        return new ProviderAvailabilityDto(
            true,
            request.PreferenciaSolucion,
            hasAvailability,
            hasConflict,
            JoinReasons(conflictReasons),
            BuildResolutionText(technicalReport, hasConflict, hasAvailability));
    }

    private static ProviderAssignmentDto ToAssignmentDto(CasoAsignado assignedCase) =>
        new(
            assignedCase.Id,
            assignedCase.SolicitudId,
            assignedCase.ProveedorId,
            assignedCase.Estado,
            assignedCase.AssignedBy,
            assignedCase.AssignedAt);

    private static ProviderWarrantyValidationDto ToWarrantyDto(ValidacionGarantia validation) =>
        new(
            validation.Id,
            validation.SolicitudId,
            validation.ProductoId,
            validation.IsWarrantyValid,
            validation.Reason,
            validation.ValidatedBy,
            validation.ValidatedAt);

    private static ProviderTechnicalReportDto ToTechnicalReportDto(DictamenTecnico technicalReport) =>
        new(
            technicalReport.Id,
            technicalReport.SolicitudId,
            technicalReport.Resultado,
            technicalReport.MotivoTecnico,
            technicalReport.Observaciones,
            technicalReport.IssuedBy,
            technicalReport.IssuedAt);

    private static string? JoinReasons(IReadOnlyCollection<string> reasons) =>
        reasons.Count == 0 ? null : string.Join(" ", reasons);

    private static string BuildResolutionText(DictamenTecnico technicalReport, bool hasConflict, bool? hasAvailability)
    {
        if (hasConflict)
        {
            return "Existe conflicto o falta de disponibilidad; el administrador conserva la decision final y debe resolverlo con el dictamen del proveedor.";
        }

        if (technicalReport.Resultado == ResultadoDictamen.Procede && hasAvailability != false)
        {
            return "Proveedor dio visto bueno y la disponibilidad fue validada para la decision final del administrador.";
        }

        return "El dictamen del proveedor queda registrado para decision final del administrador.";
    }

    private static string WithDetail(string prefix, string? detail) =>
        string.IsNullOrWhiteSpace(detail) ? $"{prefix}." : $"{prefix}: {detail.Trim()}";

    private static string? FirstText(params string?[] values) =>
        values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value))?.Trim();
}
