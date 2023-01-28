using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Shared.Common;

namespace KarcagS.Common.Tools.Table;

public abstract class TableBuilder<T, TKey> where T : class, IIdentified<TKey>
{
    protected DataSource<T, TKey> DataSource { get; set; } = default!;
    protected Configuration<T, TKey> Configuration { get; set; } = default!;

    public TableBuilder<T, TKey> AddDataSource(DataSource<T, TKey> dataSource)
    {
        DataSource = dataSource;

        return this;
    }

    public TableBuilder<T, TKey> AddConfiguration(Configuration<T, TKey> configuration)
    {
        Configuration = configuration;

        return this;
    }

    public abstract Table<T, TKey> Build();
}
