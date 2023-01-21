using AutoMapper;
using KarcagS.Common.Demo.DTOs;

namespace KarcagS.Common.Demo.Mappers;

public class GenderMapper : Profile
{
    public GenderMapper()
    {
        CreateMap<GenderEntry, GenderDTO>();
    }
}
