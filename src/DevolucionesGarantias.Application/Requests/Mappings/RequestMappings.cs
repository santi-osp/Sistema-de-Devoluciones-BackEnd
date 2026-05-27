using DevolucionesGarantias.Application.Requests.DTOs;
using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Application.Requests.Mappings;

public static class RequestMappings
{
    public static OrderDto ToDto(this Pedido pedido) =>
        new(
            pedido.Id,
            pedido.ClienteId,
            pedido.Numero,
            pedido.FechaCompra,
            pedido.Total.Amount,
            pedido.Total.Currency,
            pedido.Productos.Select(product => product.ToDto()).ToArray());

    public static ProductDto ToDto(this Producto product) =>
        new(
            product.Id,
            product.PedidoId,
            product.Sku,
            product.Nombre,
            product.Cantidad,
            product.FechaCompra,
            product.GarantiaHasta,
            product.PrecioUnitario.Amount,
            product.PrecioUnitario.Currency);

    public static RequestSummaryDto ToSummaryDto(this Solicitud request) =>
        new(
            request.Id,
            request.ClienteId,
            request.PedidoId,
            request.ProductoId,
            request.Tipo,
            request.EstadoActual,
            request.Motivo,
            request.CreatedAt);

    public static RequestDetailDto ToDetailDto(this Solicitud request) =>
        new(
            request.Id,
            request.ClienteId,
            request.PedidoId,
            request.ProductoId,
            request.Tipo,
            request.Motivo,
            request.Descripcion,
            request.Cantidad,
            request.PreferenciaSolucion,
            request.EstadoActual,
            request.CreatedAt,
            request.Evidencias.Select(evidence => evidence.ToDto()).ToArray(),
            request.Timeline.Select(item => item.ToDto()).ToArray());

    public static EvidenceDto ToDto(this Evidencia evidence) =>
        new(
            evidence.Id,
            evidence.SolicitudId,
            evidence.Tipo,
            evidence.Archivo.Bucket,
            evidence.Archivo.Path,
            evidence.Archivo.Url,
            evidence.NombreArchivo,
            evidence.SizeInBytes,
            evidence.UploadedAt);

    public static RequestTimelineDto ToDto(this RequestTimeline item) =>
        new(
            item.Id,
            item.SolicitudId,
            item.Evento,
            item.EstadoAnterior,
            item.EstadoNuevo,
            item.CreatedAt,
            item.CreatedBy);
}
