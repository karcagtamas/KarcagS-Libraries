using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;

namespace KarcagS.Common.Tools.Table;

public abstract class DataSource<T, TKey> where T : class, IIdentified<TKey>
{
    public abstract IEnumerable<T> LoadData(QueryModel query, Configuration<T, TKey> configuration);
    public abstract int LoadAllDataCount();
    public abstract int LoadFilteredAllDataCount(QueryModel query, Configuration<T, TKey> configuration);
}
