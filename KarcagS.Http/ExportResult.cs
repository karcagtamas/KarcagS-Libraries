namespace KarcagS.Http;

public class ExportResult
{
    public byte[] Content { get; set; } = Array.Empty<byte>();
    public string FileName { get; set; } = "";
    public string ContentType { get; set; } = "";
}
