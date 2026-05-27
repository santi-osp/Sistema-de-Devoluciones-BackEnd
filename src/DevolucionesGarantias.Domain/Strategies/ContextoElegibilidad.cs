using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Domain.Strategies;

// Strategy: permite intercambiar reglas de elegibilidad segun tipo de solicitud.
public sealed class ContextoElegibilidad
{
    private ReglaElegibilidadStrategy? _strategy;

    public ContextoElegibilidad(ReglaElegibilidadStrategy? strategy = null)
    {
        _strategy = strategy;
    }

    public void EstablecerEstrategia(ReglaElegibilidadStrategy strategy)
    {
        _strategy = strategy;
    }

    public ResultadoElegibilidad Evaluar(Pedido pedido, Producto producto, DateTimeOffset? fechaEvaluacion = null)
    {
        if (_strategy is null)
        {
            throw new BusinessRuleException("Debe establecerse una estrategia de elegibilidad.");
        }

        return _strategy.Evaluar(pedido, producto, fechaEvaluacion);
    }
}

