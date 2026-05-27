using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.ValueObjects;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Persistence.Seeders;

public sealed class RequestSeeder
{
    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (await dbContext.Solicitudes.AnyAsync(request => request.CreatedBy == "development-seed", cancellationToken))
        {
            return;
        }

        var order = await dbContext.Pedidos
            .Include(pedido => pedido.Productos)
            .FirstOrDefaultAsync(pedido => pedido.Numero == "DEV-ORDER-001", cancellationToken);
        var product = order?.Productos.FirstOrDefault();
        var providerEmail = new Email("proveedor.demo@ecommerce.com");
        var adminEmail = new Email("admin.demo@ecommerce.com");
        var provider = await dbContext.Proveedores.FirstOrDefaultAsync(user => user.Correo == providerEmail, cancellationToken);
        var admin = await dbContext.Administradores.FirstOrDefaultAsync(user => user.Correo == adminEmail, cancellationToken);

        if (order is null || product is null || provider is null || admin is null)
        {
            return;
        }

        var request = new Solicitud(
            order.ClienteId,
            order.Id,
            product.Id,
            TipoSolicitud.Garantia,
            "Producto presenta falla de funcionamiento",
            "Solicitud demo creada para validar flujo local.",
            1,
            PreferenciaSolucion.Reparacion,
            "development-seed");

        request.EnviarARevision("development-seed");
        dbContext.Solicitudes.Add(request);
        dbContext.CasosAsignados.Add(new CasoAsignado(request.Id, provider.Id, admin.Id.ToString()));
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
