namespace DevolucionesGarantias.Infrastructure.Storage;

public sealed class SupabaseStorageOptions
{
    public string Url { get; set; } = string.Empty;
    public string ServiceRoleKey { get; set; } = string.Empty;
    public string EvidenceBucket { get; set; } = "evidence-files";
    public string ReportsBucket { get; set; } = "report-files";
}
