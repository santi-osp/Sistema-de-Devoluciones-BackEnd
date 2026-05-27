using DevolucionesGarantias.Application.Reports.DTOs;
using DevolucionesGarantias.Domain.Exceptions;

namespace DevolucionesGarantias.Application.Reports.Validators;

public sealed class ReportFilterValidator
{
    public IReadOnlyCollection<string> Validate(ReportFilterDto filter)
    {
        var errors = new List<string>();

        if (filter.StartDate.HasValue && filter.EndDate.HasValue && filter.StartDate.Value > filter.EndDate.Value)
        {
            errors.Add("La fecha inicial no puede ser mayor que la fecha final.");
        }

        return errors;
    }

    public void ValidateAndThrow(ReportFilterDto filter)
    {
        var errors = Validate(filter);
        if (errors.Count > 0)
        {
            throw new BusinessRuleException(string.Join(" ", errors));
        }
    }
}
