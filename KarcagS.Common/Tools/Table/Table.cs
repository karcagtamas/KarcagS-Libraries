using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Shared.Common;

namespace KarcagS.Common.Tools.Table;

public abstract class Table<T, TKey> where T : class, IIdentified<TKey>
{
    protected readonly DataSource<T, TKey> DataSource;
    protected readonly Configuration<T, TKey> Configuration;

    public Table(DataSource<T, TKey> dataSource, Configuration<T, TKey> configuration)
    {
        DataSource = dataSource;
        Configuration = configuration;
    }

    public abstract IEnumerable<T> GetData();

    public TableMetaData<T, TKey> GetMetaData() => new(Configuration);
}
