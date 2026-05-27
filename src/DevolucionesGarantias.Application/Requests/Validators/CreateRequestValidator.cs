using DevolucionesGarantias.Application.Requests.DTOs;
using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Application.Requests.Validators;

public sealed class CreateRequestValidator
{
    public IReadOnlyCollection<string> Validate(CreateRequestDto request)
    {
        var errors = new List<string>();

        if (request.CustomerId == Guid.Empty)
        {
            errors.Add("El cliente es obligatorio.");
        }

        if (request.OrderId == Guid.Empty)
        {
            errors.Add("El pedido es obligatorio.");
        }

        if (request.ProductId == Guid.Empty)
        {
            errors.Add("El producto es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(request.Reason))
        {
            errors.Add("El motivo es obligatorio.");
        }

        if (request.Quantity <= 0)
        {
            errors.Add("La cantidad debe ser mayor que cero.");
        }

        return errors;
    }

    public void ValidateAndThrow(CreateRequestDto request)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            throw new BusinessRuleException(string.Join(" ", errors));
        }
    }
}
