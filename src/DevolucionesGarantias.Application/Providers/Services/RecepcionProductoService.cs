using DevolucionesGarantias.Application.Common.Interfaces;
using DevolucionesGarantias.Application.Providers.DTOs;
using DevolucionesGarantias.Application.Providers.Interfaces;
using DevolucionesGarantias.Application.Providers.Validators;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.ValueObjects;

namespace DevolucionesGarantias.Application.Providers.Services;

public sealed class RecepcionProductoService
{
    private readonly IProviderCaseRepository _cases;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductReceptionValidator _validator;

    public RecepcionProductoService(IProviderCaseRepository cases, IUnitOfWork unitOfWork, ProductReceptionValidator validator)
    {
        _cases = cases;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<ProductReceptionResultDto> RegisterReceptionAsync(Guid providerId, ProductReceptionDto request, CancellationToken cancellationToken = default)
    {
        _validator.ValidateAndThrow(request);

        _ = await _cases.GetAssignedCaseAsync(providerId, request.RequestId, cancellationToken)
            ?? throw new BusinessRuleException("El proveedor no esta asignado a este caso.");

        var reception = new RecepcionProducto(
            request.RequestId,
            request.Address,
            new Money(request.ShippingCost, request.Currency),
            request.ReceivedAt,
            providerId.ToString());

        await _cases.AddProductReceptionAsync(reception, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ProductReceptionResultDto(reception.Id, reception.SolicitudId, reception.Direccion, reception.CostoEnvio.Amount, reception.CostoEnvio.Currency, reception.ReceivedAt);
    }
}
