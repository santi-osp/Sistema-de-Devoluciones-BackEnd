using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Domain.States;

public sealed class SolicitudPendienteDecisionFinalAdmin : EstadoSolicitud
{
    public SolicitudPendienteDecisionFinalAdmin()
        : base(EstadoSolicitudEnum.PendienteDecisionFinalAdmin)
    {
    }

    public override void Aprobar(Solicitud solicitud, string motivo, string? updatedBy = null)
    {
        EnsureMotivo(motivo);
        solicitud.AplicarEstado(new SolicitudAprobada(), updatedBy, motivo);
    }

    public override void Rechazar(Solicitud solicitud, string motivo, string? updatedBy = null)
    {
        EnsureMotivo(motivo);
        solicitud.AplicarEstado(new SolicitudRechazada(), updatedBy, motivo);
    }

}
