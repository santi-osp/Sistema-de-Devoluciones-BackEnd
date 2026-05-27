using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Text.Json;

namespace DevolucionesGarantias.Persistence.Context;

public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    private const string DesignTimeConnectionString =
        "Host=localhost;Database=devoluciones_garantias_design_time;Username=postgres;Password=postgres;SSL Mode=Disable";

    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__NeonPostgres")
            ?? Environment.GetEnvironmentVariable("ConnectionStrings:NeonPostgres")
            ?? ReadConnectionStringFromApiSettings()
            ?? DesignTimeConnectionString;

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }

    private static string? ReadConnectionStringFromApiSettings()
    {
        var apiSettingsDirectory = FindApiSettingsDirectory();

        if (apiSettingsDirectory is null)
        {
            return null;
        }

        var baseSettings = ReadConnectionString(Path.Combine(apiSettingsDirectory, "appsettings.json"));
        var developmentSettings = ReadConnectionString(Path.Combine(apiSettingsDirectory, "appsettings.Development.json"));

        return developmentSettings ?? baseSettings;
    }

    private static string? FindApiSettingsDirectory()
    {
        var current = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (current is not null)
        {
            var fromSolutionRoot = Path.Combine(current.FullName, "src", "DevolucionesGarantias.Api");
            if (File.Exists(Path.Combine(fromSolutionRoot, "appsettings.json")))
            {
                return fromSolutionRoot;
            }

            var fromProjectSibling = Path.Combine(current.FullName, "..", "DevolucionesGarantias.Api");
            if (File.Exists(Path.Combine(fromProjectSibling, "appsettings.json")))
            {
                return Path.GetFullPath(fromProjectSibling);
            }

            current = current.Parent;
        }

        return null;
    }

    private static string? ReadConnectionString(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        using var stream = File.OpenRead(path);
        using var document = JsonDocument.Parse(stream);

        if (!document.RootElement.TryGetProperty("ConnectionStrings", out var connectionStrings))
        {
            return null;
        }

        if (!connectionStrings.TryGetProperty("NeonPostgres", out var neonPostgres))
        {
            return null;
        }

        var value = neonPostgres.GetString();
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }
}
