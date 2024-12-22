using KarcagS.Blazor.Common.Components.FileUploader;
using KarcagS.Blazor.Common.Services.Interfaces;
using MudBlazor;

namespace KarcagS.Blazor.Common.Services;

public class FileUploaderService(IDialogService dialogService) : IFileUploaderService
{
    public async Task<bool> Open(FileUploaderDialogInput input, string title)
    {
        var parameters = new DialogParameters { { "Input", input } };
        var dialog = await dialogService.ShowAsync<FileUploader>(title, parameters, new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true });
        var result = await dialog.Result;

        return !result?.Canceled ?? false;
    }
}