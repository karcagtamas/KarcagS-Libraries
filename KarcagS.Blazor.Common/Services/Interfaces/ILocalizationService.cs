namespace KarcagS.Blazor.Common.Services.Interfaces;

public interface ILocalizationService
{
    string GetValue(string key, string orElse, params string[] args);
    string GetValue(string key, params string[] args);
}
