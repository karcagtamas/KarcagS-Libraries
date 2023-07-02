using KarcagS.API.Table.Configurations;
using KarcagS.Shared.Common;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Table;
using KarcagS.Shared.Table.Enums;

namespace KarcagS.API.Table;

public abstract class Table<T, TKey> where T : class, IIdentified<TKey>
{
    protected readonly DataSource<T, TKey> DataSource;
    protected readonly Configuration<T, TKey> Configuration;

    public Table(DataSource<T, TKey> dataSource, Configuration<T, TKey> configuration)
    {
        DataSource = dataSource;
        Configuration = configuration;
    }

    public abstract Task<IEnumerable<T>> GetDataAsync(QueryModel query);

    public abstract Task<int> GetAllDataCountAsync(QueryModel query);

    public abstract Task<int> GetAllFilteredCountAsync(QueryModel query);

    public TableMetaData GetMetaData() => Configuration.Convert();

    public async Task<IEnumerable<ResultItem<TKey>>> GetDisplayDataAsync(QueryModel query) => await UnwrapItems(await GetDataAsync(query));

    public async Task<TableResult<TKey>> ConstructResultAsync(QueryModel query)
    {
        var result = new TableResult<TKey>
        {
            Items = (await GetDisplayDataAsync(query)).ToList()
        };

        if (query.IsPaginationNeeded())
        {
            result.AllItemCount = await GetAllDataCountAsync(query);
        }

        if (query.IsTextFilterNeeded())
        {
            result.FilteredAllItemCount = await GetAllFilteredCountAsync(query);
        }

        return result;
    }

    protected async Task<List<string>> UnwrapTags(T item, Column<T, TKey> column)
    {
        var list = new List<string>();

        foreach (var provider in Configuration.TagProviders)
        {
            var tag = await provider(item, column);

            if (!string.IsNullOrEmpty(tag))
            {
                list.Add(tag);
            }
        }

        return list;
    }

    protected async Task<IEnumerable<ResultItem<TKey>>> UnwrapItems(IEnumerable<T> items)
    {
        var list = new List<ResultItem<TKey>>();

        foreach (var i in items)
        {
            var item = new ResultItem<TKey>
            {
                ItemKey = i.Id
            };

            var dict = new Dictionary<string, ItemValue>();

            Configuration.Columns.ForEach(async col =>
            {
                dict.Add(col.Key, new ItemValue
                {
                    Value = await GetFormattedValue(col, i),
                    Tags = await UnwrapTags(i, col),
                    ActionDisabled = await Configuration.IsActionsDisabled(i, col)
                });
            });

            item.Values = dict;

            item.ClickDisabled = await Configuration.ClickDisableOn(i);

            list.Add(item);
        }

        return list;
    }

    private static async Task<string> GetFormattedValue(Column<T, TKey> column, T obj)
    {
        var value = await column.ValueGetter(obj);

        if (column.Formatter == ColumnFormatter.Text)
        {
            return value?.ToString() ?? "";
        }

        if (column.Formatter == ColumnFormatter.Number)
        {
            if (value is long? || value is int? || value is decimal?)
            {
                return value?.ToString() ?? "";
            }
        }

        if (column.Formatter == ColumnFormatter.Date)
        {
            if (value is DateTime?)
            {
                return DateHelper.DateToString((DateTime?)value);
            }
        }

        if (column.Formatter == ColumnFormatter.Logic)
        {
            if (value is bool?)
            {
                if (ObjectHelper.IsNull(value))
                {
                    return "";
                }
                else
                {
                    string trueText = "True";
                    string falseText = "False";

                    if (column.FormatterArgs.Length >= 2)
                    {
                        trueText = column.FormatterArgs[0];
                        falseText = column.FormatterArgs[1];
                    }

                    return (bool)value ? trueText : falseText;
                }
            }
        }

        return value?.ToString() ?? "";
    }
}