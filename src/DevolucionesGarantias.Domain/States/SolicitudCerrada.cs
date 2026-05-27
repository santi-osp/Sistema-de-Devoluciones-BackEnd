using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Domain.States;

public sealed class SolicitudCerrada : EstadoSolicitud
{
    public SolicitudCerrada()
        : base(EstadoSolicitudEnum.Cerrada)
    {
    }
}

