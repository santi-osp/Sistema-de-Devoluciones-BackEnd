using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.ValueObjects;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class FiltroReporte
{
    public FiltroReporte(DateRange? periodo = null, EstadoSolicitudEnum? estado = null, TipoSolicitud? tipoSolicitud = null)
    {
        Periodo = periodo;
        Estado = estado;
        TipoSolicitud = tipoSolicitud;
    }

    public DateRange? Periodo { get; private set; }
    public EstadoSolicitudEnum? Estado { get; private set; }
    public TipoSolicitud? TipoSolicitud { get; private set; }
}

