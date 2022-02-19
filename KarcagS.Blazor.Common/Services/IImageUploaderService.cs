using KarcagS.Blazor.Common.Components.ImageUploader;

namespace KarcagS.Blazor.Common.Services;

public interface IImageUploaderService
{
    Task<bool> Open(ImageUploaderDialogInput input, string title);
}