using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "COP", bool allowNegative = false)
    {
        if (!allowNegative && amount < 0)
        {
            throw new BusinessRuleException("El valor monetario no puede ser negativo.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new BusinessRuleException("La moneda es obligatoria.");
        }

        Amount = amount;
        Currency = currency.Trim().ToUpperInvariant();
    }
}

