using DevolucionesGarantias.Application.Auth.Interfaces;
using DevolucionesGarantias.Application.Common.Interfaces;
using DevolucionesGarantias.Application.Requests.DTOs;
using DevolucionesGarantias.Application.Requests.Interfaces;
using DevolucionesGarantias.Application.Requests.Mappings;
using DevolucionesGarantias.Application.Requests.Validators;
using DevolucionesGarantias.Domain.Builders;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.States;
using DevolucionesGarantias.Domain.Strategies;

namespace DevolucionesGarantias.Application.Requests.Services;

public sealed class RequestsService
{
    private readonly ISolicitudRepository _requests;
    private readonly IPedidoRepository _orders;
    private readonly IProductoRepository _products;
    private readonly IUsuarioRepository _users;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notifications;
    private readonly CreateRequestValidator _validator;

    public RequestsService(
        ISolicitudRepository requests,
        IPedidoRepository orders,
        IProductoRepository products,
        IUsuarioRepository users,
        IUnitOfWork unitOfWork,
        INotificationService notifications,
        CreateRequestValidator validator)
    {
        _requests = requests;
        _orders = orders;
        _products = products;
        _users = users;
        _unitOfWork = unitOfWork;
        _notifications = notifications;
        _validator = validator;
    }

    public async Task<RequestDetailDto> CreateAsync(CreateRequestDto request, CancellationToken cancellationToken = default)
    {
        _validator.ValidateAndThrow(request);

        var customer = await _users.GetByIdAsync(request.CustomerId, cancellationToken) as Cliente
            ?? throw new BusinessRuleException("Cliente no encontrado.");
        var order = await _orders.GetByIdAsync(request.OrderId, cancellationToken)
            ?? throw new BusinessRuleException("Pedido no encontrado.");
        var product = await _products.GetByIdAsync(request.ProductId, cancellationToken)
            ?? throw new BusinessRuleException("Producto no encontrado.");

        if (order.ClienteId != customer.Id)
        {
            throw new BusinessRuleException("El pedido no pertenece al cliente indicado.");
        }

        if (await _requests.ExistsDuplicateAsync(customer.Id, order.Id, product.Id, request.Reason, request.Type, cancellationToken))
        {
            throw new DuplicateRequestException("Ya existe una solicitud para el mismo pedido, producto, motivo y tipo.");
        }

        var eligibility = EvaluateEligibility(request.Type, order, product);
        if (!eligibility.IsEligible)
        {
            throw new BusinessRuleException(eligibility.Reason);
        }

        var builder = new SolicitudBuilder()
            .ConCliente(customer)
            .ConPedido(order)
            .ConProducto(product)
            .ConTipo(request.Type)
            .ConMotivo(request.Reason)
            .ConDescripcion(request.Description)
            .ConCantidad(request.Quantity)
            .ConPreferencia(request.PreferredSolution);

        var solicitud = builder.Build(customer.Id.ToString());
        await _requests.AddAsync(solicitud, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _notifications.NotifyRequestChangedAsync(solicitud.Id, "SolicitudCreada", null, cancellationToken);

        return solicitud.ToDetailDto();
    }

    public async Task<IReadOnlyCollection<RequestSummaryDto>> ListByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var requests = await _requests.ListByCustomerAsync(customerId, cancellationToken);
        return requests.Select(request => request.ToSummaryDto()).ToArray();
    }

    public async Task<RequestDetailDto?> GetDetailAsync(Guid requestId, Guid customerId, CancellationToken cancellationToken = default)
    {
        var request = await _requests.GetByIdAsync(requestId, cancellationToken);
        return request is null || request.ClienteId != customerId ? null : request.ToDetailDto();
    }

    public async Task<RequestDetailDto> ChangeStatusAsync(Guid requestId, EstadoSolicitudEnum status, string? reason, Guid changedBy, CancellationToken cancellationToken = default)
    {
        var request = await _requests.GetByIdAsync(requestId, cancellationToken)
            ?? throw new BusinessRuleException("Solicitud no encontrada.");

        request.CambiarEstado(ToState(status), changedBy.ToString(), reason);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _notifications.NotifyRequestChangedAsync(request.Id, $"Estado{status}", null, cancellationToken);

        return request.ToDetailDto();
    }

    public async Task<EligibilityResultDto> CheckEligibilityAsync(TipoSolicitud type, Guid orderId, Guid productId, CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(orderId, cancellationToken)
            ?? throw new BusinessRuleException("Pedido no encontrado.");
        var product = await _products.GetByIdAsync(productId, cancellationToken)
            ?? throw new BusinessRuleException("Producto no encontrado.");

        var result = EvaluateEligibility(type, order, product);
        return new EligibilityResultDto(result.IsEligible, result.Reason);
    }

    private static ResultadoElegibilidad EvaluateEligibility(TipoSolicitud type, Pedido order, Producto product)
    {
        ReglaElegibilidadStrategy strategy = type == TipoSolicitud.Devolucion
            ? new ReglaElegibilidadDevolucion()
            : new ReglaElegibilidadGarantia();

        return new ContextoElegibilidad(strategy).Evaluar(order, product);
    }

    private static EstadoSolicitud ToState(EstadoSolicitudEnum status) =>
        status switch
        {
            EstadoSolicitudEnum.EnRevision => new SolicitudEnRevision(),
            EstadoSolicitudEnum.PendienteInformacion => new SolicitudPendienteInformacion(),
            EstadoSolicitudEnum.Aprobada => new SolicitudAprobada(),
            EstadoSolicitudEnum.Rechazada => new SolicitudRechazada(),
            EstadoSolicitudEnum.Cerrada => new SolicitudCerrada(),
            EstadoSolicitudEnum.EnRevisionProveedor => new SolicitudEnRevisionProveedor(),
            EstadoSolicitudEnum.PendienteDecisionFinalAdmin => new SolicitudPendienteDecisionFinalAdmin(),
            _ => throw new InvalidStateTransitionException($"No se permite cambiar explicitamente a {status}.")
        };
}
