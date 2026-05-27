using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Domain.States;

public sealed class SolicitudAprobada : EstadoSolicitud
{
    public SolicitudAprobada()
        : base(EstadoSolicitudEnum.Aprobada)
    {
    }

    public override void Cerrar(Solicitud solicitud, string? updatedBy = null)
    {
        solicitud.AplicarEstado(new SolicitudCerrada(), updatedBy, "Solicitud aprobada cerrada.");
    }
}

