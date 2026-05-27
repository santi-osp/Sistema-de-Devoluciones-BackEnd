using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Domain.States;

public sealed class SolicitudCreada : EstadoSolicitud
{
    public SolicitudCreada()
        : base(EstadoSolicitudEnum.Creada)
    {
    }

    public override void EnviarARevision(Solicitud solicitud, string? updatedBy = null)
    {
        solicitud.AplicarEstado(new SolicitudEnRevision(), updatedBy, "Solicitud enviada a revision.");
    }
}

