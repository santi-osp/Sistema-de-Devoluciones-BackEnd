using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Domain.States;

public static class EstadoSolicitudFactory
{
    public static EstadoSolicitud FromEnum(EstadoSolicitudEnum estado) =>
        estado switch
        {
            EstadoSolicitudEnum.Creada => new SolicitudCreada(),
            EstadoSolicitudEnum.EnRevision => new SolicitudEnRevision(),
            EstadoSolicitudEnum.PendienteInformacion => new SolicitudPendienteInformacion(),
            EstadoSolicitudEnum.Aprobada => new SolicitudAprobada(),
            EstadoSolicitudEnum.Rechazada => new SolicitudRechazada(),
            EstadoSolicitudEnum.Cerrada => new SolicitudCerrada(),
            _ => new SolicitudCreada()
        };
}
