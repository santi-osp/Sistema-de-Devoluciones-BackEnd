namespace DevolucionesGarantias.Application.Common.DTOs;

public sealed record PagedRequestDto(int Page = 1, int PageSize = 20)
{
    public int Skip => (Math.Max(Page, 1) - 1) * Math.Max(PageSize, 1);
}
