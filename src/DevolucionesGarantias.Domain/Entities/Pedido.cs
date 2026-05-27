using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;
using DevolucionesGarantias.Domain.ValueObjects;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class Pedido : IEntity
{
    private readonly List<Producto> _productos = [];

    public Pedido(Guid clienteId, string numero, DateTimeOffset fechaCompra, Money total)
    {
        if (clienteId == Guid.Empty)
        {
            throw new BusinessRuleException("El pedido debe pertenecer a un cliente.");
        }

        if (string.IsNullOrWhiteSpace(numero))
        {
            throw new BusinessRuleException("El numero de pedido es obligatorio.");
        }

        Id = Guid.NewGuid();
        ClienteId = clienteId;
        Numero = numero.Trim();
        FechaCompra = fechaCompra;
        Total = total;
    }

    public Guid Id { get; private set; }
    public Guid ClienteId { get; private set; }
    public string Numero { get; private set; }
    public DateTimeOffset FechaCompra { get; private set; }
    public Money Total { get; private set; }
    public IReadOnlyCollection<Producto> Productos => _productos.AsReadOnly();

    public void AgregarProducto(Producto producto)
    {
        if (producto.PedidoId != Id)
        {
            throw new BusinessRuleException("El producto no pertenece a este pedido.");
        }

        _productos.Add(producto);
    }

    public bool ContieneProducto(Guid productoId) => _productos.Any(producto => producto.Id == productoId);
}

