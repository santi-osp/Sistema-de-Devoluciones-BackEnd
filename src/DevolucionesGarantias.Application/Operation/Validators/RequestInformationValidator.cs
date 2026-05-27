using DevolucionesGarantias.Application.Operation.DTOs;
using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Application.Operation.Validators;

public sealed class RequestInformationValidator
{
    public IReadOnlyCollection<string> Validate(RequestInformationDto request)
    {
        var errors = new List<string>();

        if (request.RequestId == Guid.Empty)
        {
            errors.Add("La solicitud es obligatoria.");
        }

        if (string.IsNullOrWhiteSpace(request.Message))
        {
            errors.Add("El mensaje es obligatorio.");
        }

        if (request.Deadline <= DateTimeOffset.UtcNow)
        {
            errors.Add("El plazo debe ser futuro.");
        }

        return errors;
    }

    public void ValidateAndThrow(RequestInformationDto request)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            throw new BusinessRuleException(string.Join(" ", errors));
        }
    }
}
