using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Application.Auth.Validators;

public sealed class LoginRequestValidator
{
    public IReadOnlyCollection<string> Validate(string? email, string? password)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add("El correo es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            errors.Add("La contraseña es obligatoria.");
        }

        return errors;
    }

    public void ValidateAndThrow(string? email, string? password)
    {
        var errors = Validate(email, password);
        if (errors.Count > 0)
        {
            throw new BusinessRuleException(string.Join(" ", errors));
        }
    }
}
