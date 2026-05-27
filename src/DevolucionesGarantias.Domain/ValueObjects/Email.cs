using System.Text.RegularExpressions;
using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Domain.ValueObjects;

public sealed record Email
{
    private static readonly Regex BasicEmailRegex = new("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$", RegexOptions.Compiled);

    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !BasicEmailRegex.IsMatch(value.Trim()))
        {
            throw new BusinessRuleException("El correo electronico no tiene un formato valido.");
        }

        Value = value.Trim().ToLowerInvariant();
    }

    public override string ToString() => Value;
}

