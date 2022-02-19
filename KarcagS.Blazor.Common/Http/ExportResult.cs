namespace KarcagS.Blazor.Common.Http;

public class ExportResult
{
    public byte[] Content { get; set; } = new byte[0];
    public string FileName { get; set; } = "";
    public string ContentType { get; set; } = "";
}
