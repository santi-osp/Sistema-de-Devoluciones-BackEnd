using DevolucionesGarantias.Domain.Entities;

namespace DevolucionesGarantias.Domain.Observers;

// Observer: publica eventos de solicitud sin acoplarse a canales externos.
public sealed class SolicitudNotifier : ISolicitudSubject
{
    private readonly List<ISolicitudObserver> _observers = [];

    public void Suscribir(ISolicitudObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void Desuscribir(ISolicitudObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notificar(Solicitud solicitud, string evento)
    {
        foreach (var observer in _observers)
        {
            observer.Actualizar(solicitud, evento);
        }
    }
}

