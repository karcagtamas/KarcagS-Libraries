using KarcagS.Client.Common.Models;
using KarcagS.Http.Models;

namespace KarcagS.Client.Common.Services.Interfaces;

public interface IToasterService
{
    void Open(ToasterSettings settings);
}