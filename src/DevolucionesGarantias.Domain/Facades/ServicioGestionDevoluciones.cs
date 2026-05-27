using DevolucionesGarantias.Domain.Builders;
using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.States;

namespace DevolucionesGarantias.Domain.Facades;

// Facade: expone una coordinacion conceptual de dominio sin depender de capas tecnicas.
public sealed class ServicioGestionDevoluciones
{
    public Solicitud IniciarSolicitud(SolicitudBuilder builder, string? createdBy = null)
    {
        return builder.Build(createdBy);
    }

    public Solicitud CambiarEstadoSolicitud(Solicitud solicitud, EstadoSolicitud nuevoEstado, string? updatedBy = null, string? reason = null)
    {
        solicitud.CambiarEstado(nuevoEstado, updatedBy, reason);
        return solicitud;
    }

    public DictamenTecnico RegistrarDictamen(Guid solicitudId, ResultadoDictamen resultado, string motivoTecnico, string observaciones, string issuedBy)
    {
        return new DictamenTecnico(solicitudId, resultado, motivoTecnico, observaciones, issuedBy);
    }

    public Reporte GenerarReporte(FiltroReporte filtro, string titulo, string generatedBy)
    {
        return new Reporte(titulo, filtro, generatedBy);
    }
}
