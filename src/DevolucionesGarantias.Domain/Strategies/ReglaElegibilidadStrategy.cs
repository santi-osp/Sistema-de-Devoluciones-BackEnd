using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Domain.Strategies;

public interface ReglaElegibilidadStrategy
{
    ResultadoElegibilidad Evaluar(Pedido pedido, Producto producto, DateTimeOffset? fechaEvaluacion = null);
}

