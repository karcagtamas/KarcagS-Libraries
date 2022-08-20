using KarcagS.Shared.Common;

namespace KarcagS.Common.Tools.Table;

public abstract class DataSource<T, TKey> where T : class, IIdentified<TKey>
{
    public abstract IEnumerable<T> LoadData(TableOptions options);
}
