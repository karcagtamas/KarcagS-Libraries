using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;

namespace KarcagS.Common.Demo;

public class GenderService : MapperRepository<GenderEntry, int, string>, IGenderService
{
    public GenderService(DemoContext context, ILoggerService loggerService, IUtilsService<string> utilsService, IMapper mapper) : base(context, loggerService, utilsService, mapper, "Gender")
    {
    }
}

public interface IGenderService : IMapperRepository<GenderEntry, int> { }
