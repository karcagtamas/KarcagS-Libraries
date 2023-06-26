using AutoMapper;
using KarcagS.API.Repository;
using KarcagS.API.Shared.Services;

namespace KarcagS.Common.Demo;

public class GenderService : MapperRepository<GenderEntry, int>, IGenderService
{
    public GenderService(ILoggerService loggerService, IMapper mapper, EFPersistence<DemoContext, string> persistence) : base(loggerService, mapper, persistence, "Gender")
    {
    }
}

public interface IGenderService : IMapperRepository<GenderEntry, int>
{
}