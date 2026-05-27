using DevolucionesGarantias.Application.Common.Interfaces;

namespace DevolucionesGarantias.Infrastructure.Notifications;

public sealed class EmailNotificationService : INotificationService
{
    public Task NotifyRequestChangedAsync(
        Guid requestId,
        string eventName,
        IReadOnlyDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
