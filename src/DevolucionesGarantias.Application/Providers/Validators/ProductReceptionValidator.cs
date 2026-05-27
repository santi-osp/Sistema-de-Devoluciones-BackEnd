using DevolucionesGarantias.Application.Providers.DTOs;
using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Application.Providers.Validators;

public sealed class ProductReceptionValidator
{
    public IReadOnlyCollection<string> Validate(ProductReceptionDto request)
    {
        var errors = new List<string>();

        if (request.RequestId == Guid.Empty)
        {
            errors.Add("La solicitud es obligatoria.");
        }

        if (string.IsNullOrWhiteSpace(request.Address))
        {
            errors.Add("La direccion de recepcion es obligatoria.");
        }

        if (request.ShippingCost < 0)
        {
            errors.Add("El costo de envio no puede ser negativo.");
        }

        if (string.IsNullOrWhiteSpace(request.Currency))
        {
            errors.Add("La moneda es obligatoria.");
        }

        return errors;
    }

    public void ValidateAndThrow(ProductReceptionDto request)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            throw new BusinessRuleException(string.Join(" ", errors));
        }
    }
}
