using DevolucionesGarantias.Persistence.Context;

namespace DevolucionesGarantias.Persistence.Seeders;

public sealed class DatabaseSeeder(
    RoleSeeder roleSeeder,
    UserSeeder userSeeder,
    RequestCatalogSeeder requestCatalogSeeder,
    OrderProductSeeder orderProductSeeder,
    RequestSeeder requestSeeder,
    ReportSeeder reportSeeder)
{
    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        await roleSeeder.SeedAsync(dbContext, cancellationToken);
        await userSeeder.SeedAsync(dbContext, cancellationToken);
        await requestCatalogSeeder.SeedAsync(dbContext, cancellationToken);
        await orderProductSeeder.SeedAsync(dbContext, cancellationToken);
        await requestSeeder.SeedAsync(dbContext, cancellationToken);
        await reportSeeder.SeedAsync(dbContext, cancellationToken);
    }
}
