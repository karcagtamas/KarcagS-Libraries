using KarcagS.Client.Common.Services.Interfaces;
using KarcagS.Http.Enums;
using KarcagS.Http.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KarcagS.Blazor.Common.Services;

public class BlazorToasterService : IToasterService
{
    private readonly ISnackbar snackbar;

    /// <summary>
    /// Init Toaster Service
    /// </summary>
    /// <param name="snackbar">Snackbar service</param>
    public BlazorToasterService(ISnackbar snackbar)
    {
        this.snackbar = snackbar;
    }

    /// <summary>
    /// Open toaster
    /// </summary>
    /// <param name="settings">Toaster Settings</param>
    public void Open(ToasterSettings settings) => snackbar.Add(GenerateString(settings), GetType(settings));

    private static MarkupString GenerateString(ToasterSettings settings) => new($"<h5>{settings.Caption}</h5><h6>{settings.Message}</h6>");

    private static Severity GetType(ToasterSettings settings)
    {
        var type = settings.Type switch
        {
            ToasterType.Success => Severity.Success,
            ToasterType.Error => Severity.Error,
            ToasterType.Warning => Severity.Warning,
            ToasterType.Info => Severity.Info,
            _ => Severity.Normal
        };

        return type;
    }
}