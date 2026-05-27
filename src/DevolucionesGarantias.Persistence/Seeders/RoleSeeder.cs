using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Persistence.Seeders;

public sealed class RoleSeeder
{
    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        var existingRoles = await dbContext.Roles
            .Select(role => role.Tipo)
            .ToListAsync(cancellationToken);

        foreach (var roleType in Enum.GetValues<RolUsuario>())
        {
            if (!existingRoles.Contains(roleType))
            {
                dbContext.Roles.Add(new Rol(roleType));
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
