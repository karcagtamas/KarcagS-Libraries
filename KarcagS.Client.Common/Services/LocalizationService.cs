using KarcagS.Client.Common.Services.Interfaces;
using KarcagS.Shared.Localization;

namespace KarcagS.Client.Common.Services;

public class LocalizationService : ILocalizationService
{
    public string GetValue(string key, string orElse, params string[] args)
    {
        var localizer = LibraryLocalizer.GetInstance();

        return localizer.IsRegistered() ? localizer.GetValue(key, args) : string.Format(orElse, args);
    }

    public string GetValue(string key, params string[] args)
    {
        return GetValue(key, key, args);
    }
}