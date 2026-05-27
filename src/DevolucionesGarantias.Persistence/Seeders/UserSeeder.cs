using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.ValueObjects;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Persistence.Seeders;

public sealed class UserSeeder
{
    private const string DevelopmentPasswordHash = "PBKDF2-SHA256:100000:REVWU0VFRC1TQUxULTAwMQ==:28RS6YzZ8kCL/R1EaAW2b/+Aw+GvdkhVamSbCcZwpX8=";

    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        // Seed solo para desarrollo: no representa credenciales productivas.
        await EnsureUserAsync(
            dbContext,
            new Cliente("Cliente Demo", new Email("cliente.demo@ecommerce.com"), DevelopmentPasswordHash, "development-seed"),
            RolUsuario.Cliente,
            cancellationToken);

        await EnsureUserAsync(
            dbContext,
            new Administrador("Admin Demo", new Email("admin.demo@ecommerce.com"), DevelopmentPasswordHash, "development-seed"),
            RolUsuario.Administrador,
            cancellationToken);

        await EnsureUserAsync(
            dbContext,
            new Proveedor("Proveedor Demo", new Email("proveedor.demo@ecommerce.com"), DevelopmentPasswordHash, "development-seed"),
            RolUsuario.Proveedor,
            cancellationToken);

        await EnsureUserAsync(
            dbContext,
            new Administrador("Analista Demo", new Email("analista.demo@ecommerce.com"), DevelopmentPasswordHash, "development-seed"),
            RolUsuario.Analista,
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task EnsureUserAsync(
        AppDbContext dbContext,
        Usuario user,
        RolUsuario roleType,
        CancellationToken cancellationToken)
    {
        var existingEmails = await dbContext.Usuarios
            .Select(existing => existing.Correo)
            .ToListAsync(cancellationToken);

        if (existingEmails.Contains(user.Correo))
        {
            return;
        }

        var role = await dbContext.Roles.FirstAsync(existing => existing.Tipo == roleType, cancellationToken);
        user.AgregarRol(role);
        dbContext.Usuarios.Add(user);
    }
}
