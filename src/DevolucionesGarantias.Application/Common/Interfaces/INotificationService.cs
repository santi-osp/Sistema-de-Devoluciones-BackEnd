namespace DevolucionesGarantias.Application.Common.Interfaces;

public interface INotificationService
{
    Task NotifyRequestChangedAsync(
        Guid requestId,
        string eventName,
        IReadOnlyDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default);
}
