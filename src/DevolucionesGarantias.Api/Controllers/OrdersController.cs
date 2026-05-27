using DevolucionesGarantias.Application.Common.Security;
using DevolucionesGarantias.Application.Requests.DTOs;
using DevolucionesGarantias.Application.Requests.Services;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevolucionesGarantias.Api.Controllers;

[Route("api")]
[Authorize(Roles = RoleNames.Cliente)]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "Orders")]
public sealed class OrdersController : ApiControllerBase
{
    private readonly OrdersService _ordersService;
    private readonly RequestsService _requestsService;

    public OrdersController(OrdersService ordersService, RequestsService requestsService)
    {
        _ordersService = ordersService;
        _requestsService = requestsService;
    }

    [HttpGet("orders")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<OrderDto>>>> List(CancellationToken cancellationToken)
    {
        var orders = await _ordersService.ListByCustomerAsync(CurrentUserId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<OrderDto>>.Ok(orders));
    }

    [HttpGet("orders/{orderId:guid}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> Get(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _ordersService.GetAsync(orderId, CurrentUserId, cancellationToken);
        return order is null ? NotFound(ApiResponse<object>.Fail("Pedido no encontrado.")) : Ok(ApiResponse<OrderDto>.Ok(order));
    }

    [HttpGet("orders/{orderId:guid}/products")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<ProductDto>>>> Products(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _ordersService.GetAsync(orderId, CurrentUserId, cancellationToken);
        return order is null
            ? NotFound(ApiResponse<object>.Fail("Pedido no encontrado."))
            : Ok(ApiResponse<IReadOnlyCollection<ProductDto>>.Ok(order.Products));
    }

    [HttpGet("products/{productId:guid}/eligibility")]
    public async Task<ActionResult<ApiResponse<EligibilityResultDto>>> Eligibility(
        Guid productId,
        [FromQuery] Guid orderId,
        [FromQuery] TipoSolicitud type,
        CancellationToken cancellationToken)
    {
        var result = await _requestsService.CheckEligibilityAsync(type, orderId, productId, cancellationToken);
        return Ok(ApiResponse<EligibilityResultDto>.Ok(result));
    }
}
