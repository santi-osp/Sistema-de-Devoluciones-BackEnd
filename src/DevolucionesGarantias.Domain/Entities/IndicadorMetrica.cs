using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class IndicadorMetrica : IEntity
{
    public IndicadorMetrica(Guid reporteId, string nombre, decimal valor, string unidad)
    {
        if (reporteId == Guid.Empty)
        {
            throw new BusinessRuleException("La metrica debe asociarse a un reporte.");
        }

        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new BusinessRuleException("El nombre de la metrica es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(unidad))
        {
            throw new BusinessRuleException("La unidad de la metrica es obligatoria.");
        }

        Id = Guid.NewGuid();
        ReporteId = reporteId;
        Nombre = nombre.Trim();
        Valor = valor;
        Unidad = unidad.Trim();
    }

    public Guid Id { get; private set; }
    public Guid ReporteId { get; private set; }
    public string Nombre { get; private set; }
    public decimal Valor { get; private set; }
    public string Unidad { get; private set; }
}

