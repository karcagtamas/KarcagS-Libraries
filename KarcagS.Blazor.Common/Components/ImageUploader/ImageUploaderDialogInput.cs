namespace KarcagS.Blazor.Common.Components.ImageUploader;

public class ImageUploaderDialogInput
{
    public Func<byte[], Task<bool>> ImageUpload { get; set; } = null!;
}
