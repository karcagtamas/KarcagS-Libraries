using KarcagS.API.Table.Configurations;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;

namespace KarcagS.API.Table;

public abstract class DataSource<T, TKey> where T : class, IIdentified<TKey>
{
    public abstract Task<IEnumerable<T>> LoadDataAsync(QueryModel query, Configuration<T, TKey> configuration);
    public abstract Task<long> LoadAllDataCountAsync(QueryModel query);
    public abstract Task<long> LoadFilteredAllDataCountAsync(QueryModel query, Configuration<T, TKey> configuration);
}