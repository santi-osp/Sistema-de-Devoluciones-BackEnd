using DevolucionesGarantias.Persistence.Context;
using DevolucionesGarantias.Persistence.Seeders;

namespace DevolucionesGarantias.Api.Extensions;

public static class DatabaseSeederExtensions
{
    public static async Task UseDevelopmentDatabaseSeederAsync(this WebApplication app, CancellationToken cancellationToken = default)
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }

        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();

        await seeder.SeedAsync(dbContext, cancellationToken);
    }
}
