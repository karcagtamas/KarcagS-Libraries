namespace KarcagS.Blazor.Common.Components.FileUploader;

public class FileUploaderResult
{
    public List<FileUploaderFile> Files { get; set; } = new();

    public class FileUploaderFile
    {
        public string Name { get; set; } = default!;
        public byte[] Content { get; set; } = default!;
        public long Size { get; set; }
        public string Extension { get; set; } = default!;
    }
}
