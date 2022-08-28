using KarcagS.Blazor.Common.Components.FileUploader;

namespace KarcagS.Blazor.Common.Services.Interfaces;

public interface IFileUploaderService
{
    Task<bool> Open(FileUploaderDialogInput input, string title);
}