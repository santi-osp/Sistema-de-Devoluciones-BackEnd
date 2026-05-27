using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class Reporte : IEntity
{
    private readonly List<IndicadorMetrica> _metricas = [];
    private readonly List<ArchivoExportado> _archivos = [];

    public Reporte(string titulo, FiltroReporte filtro, string generatedBy)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new BusinessRuleException("El titulo del reporte es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(generatedBy))
        {
            throw new BusinessRuleException("Debe registrarse quien genera el reporte.");
        }

        Id = Guid.NewGuid();
        Titulo = titulo.Trim();
        Filtro = filtro;
        GeneratedBy = generatedBy.Trim();
        GeneratedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Titulo { get; private set; }
    public FiltroReporte Filtro { get; private set; }
    public string GeneratedBy { get; private set; }
    public DateTimeOffset GeneratedAt { get; private set; }
    public IReadOnlyCollection<IndicadorMetrica> Metricas => _metricas.AsReadOnly();
    public IReadOnlyCollection<ArchivoExportado> Archivos => _archivos.AsReadOnly();

    public void AgregarMetrica(IndicadorMetrica metrica)
    {
        _metricas.Add(metrica);
    }

    public void RegistrarArchivo(ArchivoExportado archivo)
    {
        _archivos.Add(archivo);
    }
}

