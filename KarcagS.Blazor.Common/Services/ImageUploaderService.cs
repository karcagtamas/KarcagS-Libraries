using KarcagS.Blazor.Common.Components.ImageUploader;
using MudBlazor;

namespace KarcagS.Blazor.Common.Services;

public class ImageUploaderService : IImageUploaderService
{
    private readonly IDialogService dialogService;

    public ImageUploaderService(IDialogService dialogService)
    {
        this.dialogService = dialogService;
    }

    public async Task<bool> Open(ImageUploaderDialogInput input, string title)
    {
        var parameters = new DialogParameters { { "Input", input } };
        var dialog = dialogService.Show<ImageUploader>(title, parameters);
        var result = await dialog.Result;

        return !result.Cancelled;
    }
}