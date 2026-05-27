using DevolucionesGarantias.Persistence.Context;

namespace DevolucionesGarantias.Persistence.Seeders;

public sealed class RequestCatalogSeeder
{
    public Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
