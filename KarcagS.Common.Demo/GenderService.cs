using AutoMapper;
using KarcagS.API.Repository;
using KarcagS.API.Shared.Services;

namespace KarcagS.Common.Demo;

public class GenderService(ILoggerService loggerService, IMapper mapper, EFPersistence<DemoContext, string> persistence)
    : MapperRepository<GenderEntry, int>(loggerService, mapper, persistence, "Gender"), IGenderService;

public interface IGenderService : IMapperRepository<GenderEntry, int>;