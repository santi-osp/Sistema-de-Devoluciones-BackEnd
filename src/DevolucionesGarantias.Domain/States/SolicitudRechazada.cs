using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Domain.States;

public sealed class SolicitudRechazada : EstadoSolicitud
{
    public SolicitudRechazada()
        : base(EstadoSolicitudEnum.Rechazada)
    {
    }

    public override void Cerrar(Solicitud solicitud, string? updatedBy = null)
    {
        solicitud.AplicarEstado(new SolicitudCerrada(), updatedBy, "Solicitud rechazada cerrada.");
    }
}

