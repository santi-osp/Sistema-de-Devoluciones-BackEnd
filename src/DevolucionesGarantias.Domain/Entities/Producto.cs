using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;
using DevolucionesGarantias.Domain.ValueObjects;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class Producto : IEntity
{
    public Producto(Guid pedidoId, string sku, string nombre, int cantidad, DateTimeOffset fechaCompra, DateTimeOffset garantiaHasta, Money precioUnitario)
    {
        if (pedidoId == Guid.Empty)
        {
            throw new BusinessRuleException("El producto debe asociarse a un pedido.");
        }

        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new BusinessRuleException("El SKU es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new BusinessRuleException("El nombre del producto es obligatorio.");
        }

        if (cantidad <= 0)
        {
            throw new BusinessRuleException("La cantidad del producto debe ser mayor que cero.");
        }

        Id = Guid.NewGuid();
        PedidoId = pedidoId;
        Sku = sku.Trim();
        Nombre = nombre.Trim();
        Cantidad = cantidad;
        FechaCompra = fechaCompra;
        GarantiaHasta = garantiaHasta;
        PrecioUnitario = precioUnitario;
    }

    public Guid Id { get; private set; }
    public Guid PedidoId { get; private set; }
    public string Sku { get; private set; }
    public string Nombre { get; private set; }
    public int Cantidad { get; private set; }
    public DateTimeOffset FechaCompra { get; private set; }
    public DateTimeOffset GarantiaHasta { get; private set; }
    public Money PrecioUnitario { get; private set; }
}

