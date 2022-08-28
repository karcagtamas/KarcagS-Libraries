using KarcagS.Blazor.Common.Services.Interfaces;
using KarcagS.Shared.Localization;

namespace KarcagS.Blazor.Common.Services;

public class LocalizationService : ILocalizationService
{
    public string GetValue(string key, string orElse, params string[] args)
    {
        var localizer = LibraryLocalizer.GetInstance();

        if (localizer.IsRegistered())
        {
            return localizer.GetValue(key, args);
        }

        return string.Format(orElse, args);
    }

    public string GetValue(string key, params string[] args)
    {
        return GetValue(key, key, args);
    }
}
