using DevolucionesGarantias.Application.Common.Interfaces;

namespace DevolucionesGarantias.Infrastructure.Notifications;

public sealed class InMemoryNotificationService : INotificationService
{
    private readonly List<NotificationEvent> _events = [];

    public Task NotifyRequestChangedAsync(
        Guid requestId,
        string eventName,
        IReadOnlyDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default)
    {
        _events.Add(new NotificationEvent(requestId, eventName, metadata, DateTimeOffset.UtcNow));
        return Task.CompletedTask;
    }

    public IReadOnlyCollection<NotificationEvent> Events => _events.AsReadOnly();
}

public sealed record NotificationEvent(
    Guid RequestId,
    string EventName,
    IReadOnlyDictionary<string, string>? Metadata,
    DateTimeOffset OccurredAt);
