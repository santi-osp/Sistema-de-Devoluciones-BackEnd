using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Domain.States;

public sealed class SolicitudEnRevisionProveedor : EstadoSolicitud
{
    public SolicitudEnRevisionProveedor()
        : base(EstadoSolicitudEnum.EnRevisionProveedor)
    {
    }

    public override void CompletarRevisionProveedor(Solicitud solicitud, string motivo, string? updatedBy = null)
    {
        EnsureMotivo(motivo);
        solicitud.AplicarEstado(new SolicitudPendienteDecisionFinalAdmin(), updatedBy, motivo);
    }
}
