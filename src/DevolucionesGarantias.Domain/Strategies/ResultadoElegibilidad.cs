namespace DevolucionesGarantias.Domain.Strategies;

public sealed record ResultadoElegibilidad(bool IsEligible, string Reason)
{
    public static ResultadoElegibilidad Eligible(string reason = "Producto elegible.") => new(true, reason);

    public static ResultadoElegibilidad NotEligible(string reason) => new(false, reason);
}

