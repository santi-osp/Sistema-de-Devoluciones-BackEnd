using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.ValueObjects;
using DevolucionesGarantias.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Persistence.Seeders;

public sealed class ReportSeeder
{
    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (await dbContext.Reportes.AnyAsync(report => report.Titulo == "Reporte demo desarrollo", cancellationToken))
        {
            return;
        }

        var adminEmail = new Email("admin.demo@ecommerce.com");
        var admin = await dbContext.Administradores.FirstOrDefaultAsync(user => user.Correo == adminEmail, cancellationToken);
        if (admin is null)
        {
            return;
        }

        var report = new Reporte("Reporte demo desarrollo", new FiltroReporte(), admin.Id.ToString());
        report.AgregarMetrica(new IndicadorMetrica(report.Id, "Solicitudes demo", 1, "casos"));
        report.AgregarMetrica(new IndicadorMetrica(report.Id, "Garantias demo", 1, "casos"));

        dbContext.Reportes.Add(report);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
