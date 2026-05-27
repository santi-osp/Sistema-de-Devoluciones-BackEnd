using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Domain.Observers;

public interface ISolicitudObserver
{
    void Actualizar(Solicitud solicitud, string evento);
}

