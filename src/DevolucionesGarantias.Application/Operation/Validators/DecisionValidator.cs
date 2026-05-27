using DevolucionesGarantias.Application.Operation.DTOs;
using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Application.Operation.Validators;

public sealed class DecisionValidator
{
    public IReadOnlyCollection<string> Validate(DecisionDto request)
    {
        var errors = new List<string>();

        if (request.RequestId == Guid.Empty)
        {
            errors.Add("La solicitud es obligatoria.");
        }

        if (string.IsNullOrWhiteSpace(request.Reason))
        {
            errors.Add("Toda decision debe tener motivo.");
        }

        return errors;
    }

    public void ValidateAndThrow(DecisionDto request)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            throw new BusinessRuleException(string.Join(" ", errors));
        }
    }
}
