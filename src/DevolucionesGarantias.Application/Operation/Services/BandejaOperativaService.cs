using DevolucionesGarantias.Application.Operation.DTOs;
using DevolucionesGarantias.Application.Operation.Interfaces;
using DevolucionesGarantias.Application.Providers.DTOs;
using DevolucionesGarantias.Application.Providers.Services;
using DevolucionesGarantias.Application.Requests.Mappings;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Application.Operation.Services;

public sealed class BandejaOperativaService
{
    private readonly IOperationRepository _operations;
    private readonly IComentarioRepository _comments;
    private readonly ProviderReviewProjectionService _providerReview;

    public BandejaOperativaService(
        IOperationRepository operations,
        IComentarioRepository comments,
        ProviderReviewProjectionService providerReview)
    {
        _operations = operations;
        _comments = comments;
        _providerReview = providerReview;
    }

    public async Task<OperationDashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var requests = await _operations.ListAsync(null, cancellationToken);
        var recent = requests
            .OrderByDescending(request => request.CreatedAt)
            .Take(20)
            .Select(ToOperationRequestDto)
            .ToArray();

        return new OperationDashboardDto(
            requests.Count(request => request.EstadoActual is not EstadoSolicitudEnum.Cerrada),
            requests.Count(request => request.EstadoActual == EstadoSolicitudEnum.PendienteInformacion),
            requests.Count(request => request.EstadoActual is EstadoSolicitudEnum.EnRevision or EstadoSolicitudEnum.EnRevisionProveedor or EstadoSolicitudEnum.PendienteDecisionFinalAdmin),
            requests.Count(request => request.EstadoActual == EstadoSolicitudEnum.Aprobada),
            requests.Count(request => request.EstadoActual == EstadoSolicitudEnum.Rechazada),
            recent);
    }

    public async Task<IReadOnlyCollection<OperationRequestDto>> ListAsync(EstadoSolicitudEnum? status = null, CancellationToken cancellationToken = default)
    {
        var requests = await _operations.ListAsync(status, cancellationToken);
        return requests.Select(ToOperationRequestDto).ToArray();
    }

    public async Task<OperationRequestDetailDto> GetDetailAsync(Guid requestId, CancellationToken cancellationToken = default)
    {
        var request = await _operations.GetByIdAsync(requestId, cancellationToken)
            ?? throw new BusinessRuleException("Solicitud no encontrada.");
        var comments = await _comments.ListByRequestAsync(requestId, cancellationToken);
        var providerReview = await BuildProviderReviewAsync(request, cancellationToken);

        return new OperationRequestDetailDto(
            request.Id,
            request.ClienteId,
            request.PedidoId,
            request.ProductoId,
            request.Tipo,
            request.EstadoActual,
            request.Motivo,
            request.Descripcion,
            request.Cantidad,
            request.PreferenciaSolucion,
            request.Evidencias.Select(evidence => evidence.ToDto()).ToArray(),
            comments.Select(ToCommentDto).ToArray(),
            providerReview);
    }

    private static OperationRequestDto ToOperationRequestDto(Solicitud request) =>
        new(request.Id, request.ClienteId, request.Tipo, request.EstadoActual, request.Motivo, request.CreatedAt);

    private static InternalCommentDto ToCommentDto(ComentarioInterno comment) =>
        new(comment.Id, comment.SolicitudId, comment.Texto, comment.Autor, comment.VisibleToCustomer, comment.CreatedAt);

    private async Task<ProviderReviewDto?> BuildProviderReviewAsync(Solicitud request, CancellationToken cancellationToken)
    {
        var assignedCase = await _operations.GetAssignedCaseByRequestAsync(request.Id, cancellationToken);
        var warrantyValidation = await _operations.GetWarrantyValidationByRequestAsync(request.Id, cancellationToken);
        var technicalReport = await _operations.GetTechnicalReportByRequestAsync(request.Id, cancellationToken);

        if (assignedCase is null && warrantyValidation is null && technicalReport is null)
        {
            return null;
        }

        return await _providerReview.BuildAsync(request, assignedCase, warrantyValidation, technicalReport, cancellationToken);
    }
}
