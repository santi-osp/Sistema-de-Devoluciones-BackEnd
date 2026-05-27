using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Domain.States;

public sealed class SolicitudPendienteInformacion : EstadoSolicitud
{
    public SolicitudPendienteInformacion()
        : base(EstadoSolicitudEnum.PendienteInformacion)
    {
    }

    public override void EnviarARevision(Solicitud solicitud, string? updatedBy = null)
    {
        solicitud.AplicarEstado(new SolicitudEnRevision(), updatedBy, "Informacion recibida; solicitud vuelve a revision.");
    }

    public override void Rechazar(Solicitud solicitud, string motivo, string? updatedBy = null)
    {
        EnsureMotivo(motivo);
        solicitud.AplicarEstado(new SolicitudRechazada(), updatedBy, motivo);
    }
}

