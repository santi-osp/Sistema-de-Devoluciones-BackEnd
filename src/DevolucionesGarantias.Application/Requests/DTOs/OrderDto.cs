namespace DevolucionesGarantias.Application.Requests.DTOs;

public sealed record OrderDto(
    Guid Id,
    Guid CustomerId,
    string Number,
    DateTimeOffset PurchaseDate,
    decimal TotalAmount,
    string Currency,
    IReadOnlyCollection<ProductDto> Products);
