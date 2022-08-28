using KarcagS.Blazor.Common.Services;
using KarcagS.Blazor.Common.Services.Interfaces;
using KarcagS.Shared.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace KarcagS.Blazor.Common.Localization;

public static class LocalizationExtensions
{
    public static void RegisterLibraryLocalizator(IStringLocalizer localizer)
    {
        var libLocalizer = LibraryLocalizer.GetInstance();

        libLocalizer.AddLocalizer(localizer);
    }

    public static IServiceCollection AddLibraryLocalization(this IServiceCollection services)
    {
        services.AddScoped<ILocalizationService, LocalizationService>();

        return services;
    }
}
