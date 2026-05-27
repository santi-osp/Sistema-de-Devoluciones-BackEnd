using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Domain.Observers;

public interface ISolicitudSubject
{
    void Suscribir(ISolicitudObserver observer);
    void Desuscribir(ISolicitudObserver observer);
    void Notificar(Solicitud solicitud, string evento);
}

