using KarcagS.Blazor.Common.Models;

namespace KarcagS.Blazor.Common.Services.Interfaces;

public interface IToasterService
{
    void Open(ToasterSettings settings);
}
