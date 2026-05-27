using DevolucionesGarantias.Domain.Entities;
using DevolucionesGarantias.Domain.Enums;

namespace DevolucionesGarantias.Application.Operation.Interfaces;

public interface IOperationRepository
{
    Task<IReadOnlyCollection<Solicitud>> ListAsync(EstadoSolicitudEnum? status = null, CancellationToken cancellationToken = default);
    Task<Solicitud?> GetByIdAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task AddInformationRequestAsync(SolicitudInformacionAdicional informationRequest, CancellationToken cancellationToken = default);
}
