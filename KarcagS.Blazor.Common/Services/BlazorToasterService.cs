using KarcagS.Client.Common.Models;
using KarcagS.Client.Common.Services.Interfaces;
using KarcagS.Http.Enums;
using KarcagS.Http.Models;
using MudBlazor;

namespace KarcagS.Blazor.Common.Services;

public class BlazorToasterService : IToasterService
{
    private readonly ISnackbar _snackbar;

    /// <summary>
    /// Init Toaster Service
    /// </summary>
    /// <param name="snackbar">Snackbar service</param>
    public BlazorToasterService(ISnackbar snackbar)
    {
        _snackbar = snackbar;
    }

    /// <summary>
    /// Open toaster
    /// </summary>
    /// <param name="settings">Toaster Settings</param>
    public void Open(ToasterSettings settings) => _snackbar.Add(GenerateString(settings), GetType(settings));

    private static string GenerateString(ToasterSettings settings) => $"<h5>{settings.Caption}</h5><h6>{settings.Message}</h6>";

    private static Severity GetType(ToasterSettings settings)
    {
        Severity type = Severity.Normal;
        switch (settings.Type)
        {
            case ToasterType.Success:
                type = Severity.Success;
                break;
            case ToasterType.Error:
                type = Severity.Error;
                break;
            case ToasterType.Warning:
                type = Severity.Warning;
                break;
            case ToasterType.Info:
                type = Severity.Info;
                break;
        }

        return type;
    }
}
