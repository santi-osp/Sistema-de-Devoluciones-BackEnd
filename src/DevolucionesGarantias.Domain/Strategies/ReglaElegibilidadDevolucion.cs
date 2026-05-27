using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Domain.Strategies;

public sealed class ReglaElegibilidadDevolucion : ReglaElegibilidadStrategy
{
    private const int ReturnWindowDays = 30;

    public ResultadoElegibilidad Evaluar(Pedido pedido, Producto producto, DateTimeOffset? fechaEvaluacion = null)
    {
        var evaluationDate = fechaEvaluacion ?? DateTimeOffset.UtcNow;

        if (!pedido.ContieneProducto(producto.Id))
        {
            return ResultadoElegibilidad.NotEligible("El producto no pertenece al pedido seleccionado.");
        }

        if (producto.FechaCompra.AddDays(ReturnWindowDays) < evaluationDate)
        {
            return ResultadoElegibilidad.NotEligible("El producto esta fuera de la ventana permitida para devolucion.");
        }

        return ResultadoElegibilidad.Eligible("Producto elegible para devolucion.");
    }
}

