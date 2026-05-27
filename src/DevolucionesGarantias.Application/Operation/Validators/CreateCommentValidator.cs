using DevolucionesGarantias.Application.Operation.DTOs;
using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Application.Operation.Validators;

public sealed class CreateCommentValidator
{
    public IReadOnlyCollection<string> Validate(CreateCommentDto request)
    {
        var errors = new List<string>();

        if (request.RequestId == Guid.Empty)
        {
            errors.Add("La solicitud es obligatoria.");
        }

        if (string.IsNullOrWhiteSpace(request.Text))
        {
            errors.Add("El comentario es obligatorio.");
        }

        return errors;
    }

    public void ValidateAndThrow(CreateCommentDto request)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            throw new BusinessRuleException(string.Join(" ", errors));
        }
    }
}
