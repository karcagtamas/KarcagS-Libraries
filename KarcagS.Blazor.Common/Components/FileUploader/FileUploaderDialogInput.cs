namespace KarcagS.Blazor.Common.Components.FileUploader;

public class FileUploaderDialogInput
{
    public List<string> ImageExtensions { get; set; } = new()
    {
        "image/jpeg",
        "image/png",
        "image/gif"
    };

    public Func<FileUploaderResult, Task<bool>> ImageUpload { get; set; } = null!;
    public List<string> EnabledExtensions { get; set; } = new();
    public bool Required { get; set; } = false;
    public bool Multiple { get; set; } = false;
}
