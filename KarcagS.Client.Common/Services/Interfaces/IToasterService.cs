using KarcagS.Client.Common.Models;

namespace KarcagS.Client.Common.Services.Interfaces;

public interface IToasterService
{
    void Open(ToasterSettings settings);
}