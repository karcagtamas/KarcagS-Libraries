namespace KarcagS.Blazor.Common.Components.FileUploader;

public class FileUploaderDialogInput
{
    public static List<string> ImageExtensions { get; set; } =
    [
        "image/jpeg",
        "image/png",
        "image/gif"
    ];

    public Func<FileUploaderResult, Task<bool>> ImageUpload { get; set; } = null!;
    public List<string> EnabledExtensions { get; set; } = [];
    public bool Required { get; set; } = false;
    public bool Multiple { get; set; } = false;
    public long MaxFileSize { get; set; } = 512000; // In bytes
    public bool AllowAnyExtension { get; set; }
    public bool AllowAnyFileSize { get; set; }
}