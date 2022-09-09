using KarcagS.Common.Helpers;
using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;

namespace KarcagS.Common.Tools.Table;

public abstract class TableService<T, TKey> : ITableService<T, TKey> where T : class, IIdentified<TKey>
{
    protected Table<T, TKey>? Table { get; set; }

    public void Initialize()
    {
        Table = BuildTable();
    }

    public abstract Table<T, TKey> BuildTable();
    public abstract DataSource<T, TKey> BuildDataSource();
    public abstract Configuration<T, TKey> BuildConfiguration();

    public async Task<TableResult<TKey>> GetData(QueryModel query)
    {
        Check();

        ExceptionHelper.Check(await Authorize(query), () => new TableNotAuthorizedException());

        return Table!.ConstructResult(query);
    }

    public TableMetaData GetTableMetaData()
    {
        Check();

        return Table!.GetMetaData();
    }

    protected void Check() => ExceptionHelper.ThrowIfIsNull<Table<T, TKey>, TableException>(Table, "Table is not initialized");

    public virtual Task<bool> Authorize(QueryModel query) => Task.FromResult(true);
}
