namespace DevolucionesGarantias.Domain.Exceptions;

public sealed class UnauthorizedDomainActionException : DomainException
{
    public UnauthorizedDomainActionException(string message)
        : base(message)
    {
    }
}

