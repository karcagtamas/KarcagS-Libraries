using AutoMapper;
using KarcagS.API.Repository;
using KarcagS.API.Shared.Services;

namespace KarcagS.Common.Demo;

public class GenderService : MapperRepository<GenderEntry, int, string>, IGenderService
{
    public GenderService(DemoContext context, ILoggerService loggerService, IUserProvider<string> userProvider, IMapper mapper) : base(context, loggerService, userProvider, mapper, "Gender")
    {
    }
}

public interface IGenderService : IMapperRepository<GenderEntry, int>
{
}