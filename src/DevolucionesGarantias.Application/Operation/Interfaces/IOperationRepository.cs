using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Operation.Interfaces;

public interface IOperationRepository
{
    Task<IReadOnlyCollection<Solicitud>> ListAsync(EstadoSolicitudEnum? status = null, CancellationToken cancellationToken = default);
    Task<Solicitud?> GetByIdAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task<Proveedor?> GetProviderByIdAsync(Guid providerId, CancellationToken cancellationToken = default);
    Task<Proveedor?> GetDefaultProviderAsync(CancellationToken cancellationToken = default);
    Task<CasoAsignado?> GetAssignedCaseByRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task<ValidacionGarantia?> GetWarrantyValidationByRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task<DictamenTecnico?> GetTechnicalReportByRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task AddAssignedCaseAsync(CasoAsignado assignedCase, CancellationToken cancellationToken = default);
    Task AddInformationRequestAsync(SolicitudInformacionAdicional informationRequest, CancellationToken cancellationToken = default);
}
