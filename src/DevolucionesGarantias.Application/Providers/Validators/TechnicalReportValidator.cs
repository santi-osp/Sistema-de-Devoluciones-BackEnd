using DevolucionesGarantias.Application.Providers.DTOs;
using DevolucionesGarantias.Domain.Enums;
using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Application.Providers.Validators;

public sealed class TechnicalReportValidator
{
    public IReadOnlyCollection<string> Validate(TechnicalReportDto request)
    {
        var errors = new List<string>();

        if (request.RequestId == Guid.Empty)
        {
            errors.Add("La solicitud es obligatoria.");
        }

        if (request.Result == ResultadoDictamen.NoProcede && string.IsNullOrWhiteSpace(request.TechnicalReason))
        {
            errors.Add("Un dictamen NoProcede debe incluir motivo tecnico.");
        }

        return errors;
    }

    public void ValidateAndThrow(TechnicalReportDto request)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            throw new BusinessRuleException(string.Join(" ", errors));
        }
    }
}
