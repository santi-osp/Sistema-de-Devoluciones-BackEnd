using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Domain.Interfaces;

namespace DevolucionesGarantias.Domain.Entities;

public sealed class DictamenTecnico : IEntity
{
    public DictamenTecnico(Guid solicitudId, ResultadoDictamen resultado, string motivoTecnico, string observaciones, string issuedBy)
    {
        if (solicitudId == Guid.Empty)
        {
            throw new BusinessRuleException("El dictamen tecnico debe asociarse a una solicitud.");
        }

        if (resultado == ResultadoDictamen.NoProcede && string.IsNullOrWhiteSpace(motivoTecnico))
        {
            throw new BusinessRuleException("Un dictamen no procede debe incluir motivo tecnico.");
        }

        if (string.IsNullOrWhiteSpace(issuedBy))
        {
            throw new BusinessRuleException("Debe registrarse quien emite el dictamen tecnico.");
        }

        Id = Guid.NewGuid();
        SolicitudId = solicitudId;
        Resultado = resultado;
        MotivoTecnico = motivoTecnico?.Trim() ?? string.Empty;
        Observaciones = observaciones?.Trim() ?? string.Empty;
        IssuedBy = issuedBy.Trim();
        IssuedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid SolicitudId { get; private set; }
    public ResultadoDictamen Resultado { get; private set; }
    public string MotivoTecnico { get; private set; }
    public string Observaciones { get; private set; }
    public string IssuedBy { get; private set; }
    public DateTimeOffset IssuedAt { get; private set; }
}

