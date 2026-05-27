using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Domain.Strategies;

public sealed class ReglaElegibilidadGarantia : ReglaElegibilidadStrategy
{
    public ResultadoElegibilidad Evaluar(Pedido pedido, Producto producto, DateTimeOffset? fechaEvaluacion = null)
    {
        var evaluationDate = fechaEvaluacion ?? DateTimeOffset.UtcNow;

        if (!pedido.ContieneProducto(producto.Id))
        {
            return ResultadoElegibilidad.NotEligible("El producto no pertenece al pedido seleccionado.");
        }

        if (producto.GarantiaHasta < evaluationDate)
        {
            return ResultadoElegibilidad.NotEligible("La garantia del producto no esta vigente.");
        }

        return ResultadoElegibilidad.Eligible("Producto elegible para garantia.");
    }
}

