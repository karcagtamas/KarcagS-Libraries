namespace KarcagS.Blazor.Common.Components.FileUploader;

public class FileUploaderResult
{
    public List<FileUploaderFile> Files { get; set; } = [];

    public class FileUploaderFile
    {
        public string Name { get; set; } = null!;
        public byte[] Content { get; set; } = null!;
        public long Size { get; set; }
        public string Extension { get; set; } = null!;
    }
}
