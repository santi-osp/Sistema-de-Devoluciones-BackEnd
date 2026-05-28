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
        solicitud.AplicarEstado(new SolicitudEnRevisionProveedor(), updatedBy, "Informacion recibida; solicitud enviada a revision del proveedor.");
    }

    public override void Rechazar(Solicitud solicitud, string motivo, string? updatedBy = null)
    {
        EnsureMotivo(motivo);
        solicitud.AplicarEstado(new SolicitudRechazada(), updatedBy, motivo);
    }
}

