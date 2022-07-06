using KarcagS.Blazor.Common.Components.FileUploader;
using MudBlazor;

namespace KarcagS.Blazor.Common.Services;

public class FileUploaderService : IFileUploaderService
{
    private readonly IDialogService dialogService;

    public FileUploaderService(IDialogService dialogService)
    {
        this.dialogService = dialogService;
    }

    public async Task<bool> Open(FileUploaderDialogInput input, string title)
    {
        var parameters = new DialogParameters { { "Input", input } };
        var dialog = dialogService.Show<FileUploader>(title, parameters, new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true });
        var result = await dialog.Result;

        return !result.Cancelled;
    }
}