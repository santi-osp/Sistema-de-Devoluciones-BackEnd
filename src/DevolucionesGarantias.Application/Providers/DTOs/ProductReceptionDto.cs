namespace DevolucionesGarantias.Application.Providers.DTOs;

public sealed record ProductReceptionDto(
    Guid RequestId,
    string Address,
    decimal ShippingCost,
    string Currency,
    DateTimeOffset ReceivedAt);
