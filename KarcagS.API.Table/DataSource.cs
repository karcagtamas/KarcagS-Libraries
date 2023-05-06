using KarcagS.API.Table.Configurations;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;

namespace KarcagS.API.Table;

public abstract class DataSource<T, TKey> where T : class, IIdentified<TKey>
{
    public abstract IEnumerable<T> LoadData(QueryModel query, Configuration<T, TKey> configuration);
    public abstract int LoadAllDataCount(QueryModel query);
    public abstract int LoadFilteredAllDataCount(QueryModel query, Configuration<T, TKey> configuration);
}