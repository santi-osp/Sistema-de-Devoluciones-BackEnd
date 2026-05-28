using System.Text.Json;
using DevolucionesGarantias.Application.Common.Interfaces;
using DevolucionesGarantias.Application.Operation.DTOs;
using DevolucionesGarantias.Application.Operation.Interfaces;
using DevolucionesGarantias.Application.Operation.Validators;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Application.Operation.Services;

public sealed class RevisionOperativaService
{
    private readonly IOperationRepository _operations;
    private readonly IComentarioRepository _comments;
    private readonly IDecisionOperativaRepository _decisions;
    private readonly IAuditService _audit;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateCommentValidator _commentValidator;
    private readonly DecisionValidator _decisionValidator;
    private readonly RequestInformationValidator _informationValidator;

    public RevisionOperativaService(
        IOperationRepository operations,
        IComentarioRepository comments,
        IDecisionOperativaRepository decisions,
        IAuditService audit,
        IUnitOfWork unitOfWork,
        CreateCommentValidator commentValidator,
        DecisionValidator decisionValidator,
        RequestInformationValidator informationValidator)
    {
        _operations = operations;
        _comments = comments;
        _decisions = decisions;
        _audit = audit;
        _unitOfWork = unitOfWork;
        _commentValidator = commentValidator;
        _decisionValidator = decisionValidator;
        _informationValidator = informationValidator;
    }

    public async Task<InternalCommentDto> AddCommentAsync(CreateCommentDto request, Guid authorId, CancellationToken cancellationToken = default)
    {
        _commentValidator.ValidateAndThrow(request);

        _ = await _operations.GetByIdAsync(request.RequestId, cancellationToken)
            ?? throw new BusinessRuleException("Solicitud no encontrada.");

        var comment = new ComentarioInterno(request.RequestId, request.Text, authorId.ToString(), request.VisibleToCustomer);
        await _comments.AddAsync(comment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new InternalCommentDto(comment.Id, comment.SolicitudId, comment.Texto, comment.Autor, comment.VisibleToCustomer, comment.CreatedAt);
    }

    public async Task RequestInformationAsync(RequestInformationDto request, Guid requestedBy, CancellationToken cancellationToken = default)
    {
        _informationValidator.ValidateAndThrow(request);

        var solicitud = await _operations.GetByIdAsync(request.RequestId, cancellationToken)
            ?? throw new BusinessRuleException("Solicitud no encontrada.");
        solicitud.SolicitarInformacion(request.Message, requestedBy.ToString());

        await _operations.AddInformationRequestAsync(new SolicitudInformacionAdicional(request.RequestId, request.Message, request.Deadline, requestedBy.ToString()), cancellationToken);
        await _audit.RegisterAsync(requestedBy, "RequestInformation", nameof(Solicitud), request.RequestId.ToString(), null, request.Message, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<SendToProviderReviewResultDto> SendToReviewAsync(
        Guid requestId,
        SendToProviderReviewDto request,
        Guid updatedBy,
        CancellationToken cancellationToken = default)
    {
        var solicitud = await _operations.GetByIdAsync(requestId, cancellationToken)
            ?? throw new BusinessRuleException("Solicitud no encontrada.");

        if (solicitud.EstadoActual is not (EstadoSolicitudEnum.Creada or EstadoSolicitudEnum.PendienteInformacion))
        {
            throw new BusinessRuleException("Solo las solicitudes creadas o pendientes de informacion pueden enviarse a revision del proveedor.");
        }

        var provider = request.ProviderId.HasValue
            ? await _operations.GetProviderByIdAsync(request.ProviderId.Value, cancellationToken)
            : await _operations.GetDefaultProviderAsync(cancellationToken);

        if (provider is null)
        {
            throw new BusinessRuleException("No hay un proveedor disponible para asignar la solicitud.");
        }

        var assignedCase = await _operations.GetAssignedCaseByRequestAsync(requestId, cancellationToken);
        if (assignedCase is null)
        {
            assignedCase = new CasoAsignado(requestId, provider.Id, updatedBy.ToString());
            await _operations.AddAssignedCaseAsync(assignedCase, cancellationToken);
        }
        else if (assignedCase.ProveedorId != provider.Id)
        {
            throw new BusinessRuleException("La solicitud ya esta asignada a otro proveedor.");
        }
        else
        {
            assignedCase.CambiarEstado(EstadoAsignacionProveedor.Asignado);
        }

        solicitud.EnviarARevision(updatedBy.ToString());

        await _audit.RegisterAsync(
            updatedBy,
            "SendToProviderReview",
            nameof(Solicitud),
            requestId.ToString(),
            null,
            JsonSerializer.Serialize(new { providerId = provider.Id, status = solicitud.EstadoActual.ToString() }),
            cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new SendToProviderReviewResultDto(
            solicitud.Id,
            provider.Id,
            solicitud.EstadoActual,
            assignedCase.Estado);
    }

    public async Task<DecisionResultDto> DecideAsync(DecisionDto request, Guid decidedBy, CancellationToken cancellationToken = default)
    {
        _decisionValidator.ValidateAndThrow(request);

        var solicitud = await _operations.GetByIdAsync(request.RequestId, cancellationToken)
            ?? throw new BusinessRuleException("Solicitud no encontrada.");

        if (request.Approved)
        {
            solicitud.Aprobar(request.Reason, decidedBy.ToString());
        }
        else
        {
            solicitud.Rechazar(request.Reason, decidedBy.ToString());
        }

        var decision = new DecisionOperativa(request.RequestId, request.Approved, request.Reason, decidedBy.ToString());
        await _decisions.AddAsync(decision, cancellationToken);
        await _audit.RegisterAsync(decidedBy, "OperationalDecision", nameof(Solicitud), request.RequestId.ToString(), null, request.Reason, cancellationToken);

        var assignedCase = await _operations.GetAssignedCaseByRequestAsync(request.RequestId, cancellationToken);
        assignedCase?.CambiarEstado(EstadoAsignacionProveedor.Cerrado);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DecisionResultDto(decision.Id, decision.SolicitudId, decision.Approved, decision.Motivo, decision.DecidedAt);
    }
}
