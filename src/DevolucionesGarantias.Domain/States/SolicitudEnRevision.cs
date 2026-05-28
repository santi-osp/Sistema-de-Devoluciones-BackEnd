using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Domain.States;

public sealed class SolicitudEnRevision : EstadoSolicitud
{
    public SolicitudEnRevision()
        : base(EstadoSolicitudEnum.EnRevision)
    {
    }

    public override void Aprobar(Solicitud solicitud, string motivo, string? updatedBy = null)
    {
        EnsureMotivo(motivo);
        solicitud.AplicarEstado(new SolicitudAprobada(), updatedBy, motivo);
    }

    public override void CompletarRevisionProveedor(Solicitud solicitud, string motivo, string? updatedBy = null)
    {
        EnsureMotivo(motivo);
        solicitud.AplicarEstado(new SolicitudPendienteDecisionFinalAdmin(), updatedBy, motivo);
    }

    public override void Rechazar(Solicitud solicitud, string motivo, string? updatedBy = null)
    {
        EnsureMotivo(motivo);
        solicitud.AplicarEstado(new SolicitudRechazada(), updatedBy, motivo);
    }

    public override void SolicitarInformacion(Solicitud solicitud, string motivo, string? updatedBy = null)
    {
        EnsureMotivo(motivo);
        solicitud.AplicarEstado(new SolicitudPendienteInformacion(), updatedBy, motivo);
    }
}

