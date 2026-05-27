namespace DevolucionesGarantias.Application.Operation.DTOs;

public sealed record OperationDashboardDto(
    int TotalOpen,
    int PendingInformation,
    int InReview,
    int Approved,
    int Rejected,
    IReadOnlyCollection<OperationRequestDto> RecentRequests);
