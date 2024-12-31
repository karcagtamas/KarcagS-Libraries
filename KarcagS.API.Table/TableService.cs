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

    public async Task<Table<T, TKey>> BuildTableAsync() => await BuildTableAsync(await BuildDataSourceAsync(), await BuildConfigurationAsync());

    public virtual Task<Table<T, TKey>> BuildTableAsync(DataSource<T, TKey> dataSource, Configuration<T, TKey> configuration)
    {
        return Task.FromResult(Builder()
            .AddDataSource(dataSource)
            .AddConfiguration(configuration)
            .Build());
    }

    public abstract TableBuilder<T, TKey> Builder();

    public abstract Task<DataSource<T, TKey>> BuildDataSourceAsync();

    public virtual Configuration<T, TKey> BuildConfiguration() => Configuration<T, TKey>.Build("table").SetTitle("Table");

    public virtual Task<Configuration<T, TKey>> BuildConfigurationAsync() => Task.FromResult(BuildConfiguration());

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