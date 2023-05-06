namespace KarcagS.API.Export;

public class ExportResult
{
    public byte[] Content { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
}
