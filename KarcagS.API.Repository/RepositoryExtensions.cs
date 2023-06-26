using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KarcagS.API.Repository;

public static class RepositoryExtensions
{
    public static IEnumerable<TDest> MapTo<TDest, TSrc>(this IEnumerable<TSrc> enumerable, IMapper mapper) => mapper.Map<List<TDest>>(enumerable.ToList());

    public static IServiceCollection UseEFPersistence<TDatabaseContext, TUserKey>(this IServiceCollection services) where TDatabaseContext : DbContext
    {
        services.AddTransient<EFPersistence<TDatabaseContext, TUserKey>>();

        return services;
    }
}