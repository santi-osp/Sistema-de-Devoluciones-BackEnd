using DevolucionesGarantias.Application.Common.Interfaces;

namespace DevolucionesGarantias.Infrastructure.Time;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
