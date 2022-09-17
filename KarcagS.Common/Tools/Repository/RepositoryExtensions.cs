using AutoMapper;

namespace KarcagS.Common.Tools.Repository;

public static class RepositoryExtensions
{
    public static IEnumerable<TDest> MapTo<TDest, TSrc>(this IEnumerable<TSrc> enumerable, IMapper mapper) => mapper.Map<List<TDest>>(enumerable.ToList());
}
