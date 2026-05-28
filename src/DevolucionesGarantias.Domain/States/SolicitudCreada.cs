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
        solicitud.AplicarEstado(new SolicitudEnRevisionProveedor(), updatedBy, "Solicitud enviada a revision del proveedor.");
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

