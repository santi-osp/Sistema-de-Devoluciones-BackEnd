namespace DevolucionesGarantias.Application.Requests.DTOs;

public sealed record ProductDto(
    Guid Id,
    Guid OrderId,
    string Sku,
    string Name,
    int Quantity,
    DateTimeOffset PurchaseDate,
    DateTimeOffset WarrantyUntil,
    decimal UnitPrice,
    string Currency);
