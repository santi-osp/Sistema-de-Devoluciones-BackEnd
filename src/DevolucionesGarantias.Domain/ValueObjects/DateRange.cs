using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Domain.ValueObjects;

public sealed record DateRange
{
    public DateTimeOffset Start { get; }
    public DateTimeOffset End { get; }

    public DateRange(DateTimeOffset start, DateTimeOffset end)
    {
        if (start > end)
        {
            throw new BusinessRuleException("La fecha inicial no puede ser mayor que la fecha final.");
        }

        Start = start;
        End = end;
    }

    public bool Contains(DateTimeOffset value) => value >= Start && value <= End;
}
