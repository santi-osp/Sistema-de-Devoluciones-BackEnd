using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Reports.DTOs;

public sealed record ReportFilterDto(DateTimeOffset? StartDate, DateTimeOffset? EndDate, EstadoSolicitudEnum? Status, TipoSolicitud? RequestType);
