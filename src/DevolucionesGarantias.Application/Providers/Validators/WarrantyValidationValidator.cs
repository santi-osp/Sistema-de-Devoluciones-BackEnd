using DevolucionesGarantias.Application.Providers.DTOs;
using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Application.Providers.Validators;

public sealed class WarrantyValidationValidator
{
    public IReadOnlyCollection<string> Validate(WarrantyValidationDto request)
    {
        var errors = new List<string>();

        if (request.RequestId == Guid.Empty)
        {
            errors.Add("La solicitud es obligatoria.");
        }

        if (request.ProductId == Guid.Empty)
        {
            errors.Add("El producto es obligatorio.");
        }

        if (!request.IsWarrantyValid && string.IsNullOrWhiteSpace(request.Reason))
        {
            errors.Add("La garantia no vigente debe incluir motivo.");
        }

        return errors;
    }

    public void ValidateAndThrow(WarrantyValidationDto request)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            throw new BusinessRuleException(string.Join(" ", errors));
        }
    }
}
