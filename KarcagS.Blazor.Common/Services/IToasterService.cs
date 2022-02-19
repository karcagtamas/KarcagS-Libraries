using KarcagS.Blazor.Common.Models;

namespace KarcagS.Blazor.Common.Services;

public interface IToasterService
{
    void Open(ToasterSettings settings);
}
