namespace KarcagS.API.Export.Models;

public class ExportResult
{
    public byte[] Content { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
}
