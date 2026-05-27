using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.ValueObjects;

namespace DevolucionesGarantias.Domain.Builders;

// Builder: centraliza la creacion valida de solicitudes.
public sealed class SolicitudBuilder
{
    private Cliente? _cliente;
    private Pedido? _pedido;
    private Producto? _producto;
    private TipoSolicitud? _tipo;
    private string? _motivo;
    private string? _descripcion;
    private int _cantidad;
    private PreferenciaSolucion? _preferencia;
    private readonly List<Evidencia> _evidencias = [];
    private readonly List<Func<Guid, Evidencia>> _evidenciasPendientes = [];

    public SolicitudBuilder ConCliente(Cliente cliente)
    {
        _cliente = cliente;
        return this;
    }

    public SolicitudBuilder ConPedido(Pedido pedido)
    {
        _pedido = pedido;
        return this;
    }

    public SolicitudBuilder ConProducto(Producto producto)
    {
        _producto = producto;
        return this;
    }

    public SolicitudBuilder ConTipo(TipoSolicitud tipo)
    {
        _tipo = tipo;
        return this;
    }

    public SolicitudBuilder ConMotivo(string motivo)
    {
        _motivo = motivo;
        return this;
    }

    public SolicitudBuilder ConDescripcion(string descripcion)
    {
        _descripcion = descripcion;
        return this;
    }

    public SolicitudBuilder ConCantidad(int cantidad)
    {
        _cantidad = cantidad;
        return this;
    }

    public SolicitudBuilder ConPreferencia(PreferenciaSolucion preferencia)
    {
        _preferencia = preferencia;
        return this;
    }

    public SolicitudBuilder AgregarEvidencia(Evidencia evidencia)
    {
        _evidencias.Add(evidencia);
        return this;
    }

    public SolicitudBuilder AgregarEvidencia(TipoEvidencia tipo, FilePath archivo, string nombreArchivo, long sizeInBytes, string? uploadedBy = null)
    {
        _evidenciasPendientes.Add(solicitudId => new Evidencia(solicitudId, tipo, archivo, nombreArchivo, sizeInBytes, uploadedBy));
        return this;
    }

    public Solicitud Build(string? createdBy = null)
    {
        if (_cliente is null)
        {
            throw new BusinessRuleException("La solicitud requiere cliente.");
        }

        if (_pedido is null)
        {
            throw new BusinessRuleException("La solicitud requiere pedido.");
        }

        if (_producto is null)
        {
            throw new BusinessRuleException("La solicitud requiere producto.");
        }

        if (_pedido.ClienteId != _cliente.Id)
        {
            throw new BusinessRuleException("El pedido no pertenece al cliente indicado.");
        }

        if (!_pedido.ContieneProducto(_producto.Id))
        {
            throw new BusinessRuleException("El producto no pertenece al pedido indicado.");
        }

        if (string.IsNullOrWhiteSpace(_motivo))
        {
            throw new BusinessRuleException("El motivo de la solicitud es obligatorio.");
        }

        if (_cantidad <= 0)
        {
            throw new BusinessRuleException("La cantidad solicitada debe ser mayor que cero.");
        }

        if (_cantidad > _producto.Cantidad)
        {
            throw new BusinessRuleException("La cantidad solicitada no puede superar la cantidad comprada.");
        }

        var solicitud = new Solicitud(
            _cliente.Id,
            _pedido.Id,
            _producto.Id,
            _tipo ?? TipoSolicitud.Garantia,
            _motivo,
            _descripcion ?? string.Empty,
            _cantidad,
            _preferencia ?? PreferenciaSolucion.Cambio,
            createdBy);

        foreach (var evidencia in _evidencias)
        {
            solicitud.AgregarEvidencia(evidencia, createdBy);
        }

        foreach (var evidenciaFactory in _evidenciasPendientes)
        {
            solicitud.AgregarEvidencia(evidenciaFactory(solicitud.Id), createdBy);
        }

        return solicitud;
    }
}
