using KarcagS.API.Shared.Helpers;
using KarcagS.API.Table.Configurations;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;

namespace KarcagS.API.Table;

public abstract class TableService<T, TKey> : ITableService<T, TKey> where T : class, IIdentified<TKey>
{
    protected Table<T, TKey>? Table { get; set; }
    private bool Initialized { get; set; }

    public async Task InitializeAsync()
    {
        ExceptionHelper.Throw(Initialized, "Service already has been initialized");

        Table = await BuildTableAsync();

        Initialized = true;
    }

    protected bool IsInitialized() => Initialized;

    public abstract Task<Table<T, TKey>> BuildTableAsync();
    public abstract Task<DataSource<T, TKey>> BuildDataSourceAsync();
    public abstract Task<Configuration<T, TKey>> BuildConfigurationAsync();

    public async Task<TableResult<TKey>> GetDataAsync(QueryModel query)
    {
        await CheckAsync();

        ExceptionHelper.Check(await AuthorizeAsync(query), () => new TableNotAuthorizedException());

        return await Table!.ConstructResultAsync(query);
    }

    public async Task<TableMetaData> GetTableMetaDataAsync()
    {
        await CheckAsync();

        return Table!.GetMetaData();
    }

    protected async Task CheckAsync()
    {
        if (!Initialized)
        {
            await InitializeAsync();
        }

        ExceptionHelper.ThrowIfIsNull<Table<T, TKey>, TableException>(Table, "Table is not initialized");
    }

    public virtual Task<bool> AuthorizeAsync(QueryModel query) => Task.FromResult(true);
}