using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.ValueObjects;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Persistence.Seeders;

public sealed class OrderProductSeeder
{
    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (await dbContext.Pedidos.AnyAsync(order => order.Numero == "DEV-ORDER-001", cancellationToken))
        {
            return;
        }

        var customerEmail = new Email("cliente.demo@ecommerce.com");
        var customer = await dbContext.Clientes.FirstOrDefaultAsync(user => user.Correo == customerEmail, cancellationToken);
        if (customer is null)
        {
            return;
        }

        var order = new Pedido(customer.Id, "DEV-ORDER-001", DateTimeOffset.UtcNow.AddDays(-10), new Money(250000));
        var product = new Producto(
            order.Id,
            "DEV-SKU-001",
            "Producto demo con garantia",
            1,
            order.FechaCompra,
            DateTimeOffset.UtcNow.AddMonths(6),
            new Money(250000));

        order.AgregarProducto(product);
        dbContext.Pedidos.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
