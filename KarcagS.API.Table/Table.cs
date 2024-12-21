using KarcagS.API.Table.Configurations;
using KarcagS.Shared.Common;
using KarcagS.Shared.Helpers;
using KarcagS.Shared.Table;
using KarcagS.Shared.Table.Enums;

namespace KarcagS.API.Table;

public abstract class Table<T, TKey>(DataSource<T, TKey> dataSource, Configuration<T, TKey> configuration) where T : class, IIdentified<TKey>
{
    protected readonly DataSource<T, TKey> DataSource = dataSource;
    protected readonly Configuration<T, TKey> Configuration = configuration;

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

            foreach (var col in Configuration.Columns)
            {
                dict.Add(col.Key, new ItemValue
                {
                    Value = await GetFormattedValue(col, i),
                    Tags = await UnwrapTags(i, col),
                    ActionDisabled = await Configuration.IsActionsDisabled(i, col)
                });
            }

            item.Values = dict;

            item.ClickDisabled = await Configuration.ClickDisableOn(i);

            list.Add(item);
        }

        return list;
    }

    private static async Task<string> GetFormattedValue(Column<T, TKey> column, T obj)
    {
        var value = await column.ValueGetter(obj);

        switch (column.Formatter)
        {
            case ColumnFormatter.Text:
            case ColumnFormatter.Number when value is long or int or decimal:
                return value?.ToString() ?? "";
            case ColumnFormatter.Date when value is DateTime:
                return DateHelper.DateToString((DateTime?)value);
            case ColumnFormatter.Logic when value is bool b:
            {
                if (ObjectHelper.IsNull(value))
                {
                    return "";
                }

                var trueText = "True";
                var falseText = "False";

                if (column.FormatterArgs.Length >= 2)
                {
                    trueText = column.FormatterArgs[0];
                    falseText = column.FormatterArgs[1];
                }

                return b ? trueText : falseText;
            }
            default:
                return value?.ToString() ?? "";
        }
    }
}