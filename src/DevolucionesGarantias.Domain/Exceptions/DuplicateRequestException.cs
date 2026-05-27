namespace DevolucionesGarantias.Domain.Exceptions;

public sealed class DuplicateRequestException : BusinessRuleException
{
    public DuplicateRequestException(string message)
        : base(message)
    {
    }
}

