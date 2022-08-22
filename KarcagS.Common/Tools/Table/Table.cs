using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;
using KarcagS.Shared.Table.Enums;
using static KarcagS.Shared.Table.TableResult;
using static KarcagS.Shared.Table.TableResult.ResultItem;

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

    public abstract IEnumerable<T> GetData(QueryModel query);

    public abstract int GetAllDataCount();

    public abstract int GetAllFilteredCount(QueryModel query);

    public TableMetaData GetMetaData() => Configuration.Convert();

    public IEnumerable<ResultItem> GetDisplayData(QueryModel query)
    {
        return GetData(query)
            .Select(x =>
            {
                var item = new ResultItem
                {
                    ItemKey = x.Id?.ToString() ?? ""
                };

                var dict = new Dictionary<string, ItemValue>();

                Configuration.Columns.ForEach(col =>
                {
                    dict.Add(col.Key, new ItemValue { Value = GetFormattedValue(col, x), Tags = Configuration.TagProviders.Select(provider => provider(x, col)).Where(tag => !string.IsNullOrEmpty(tag)).ToList() });
                });

                item.Values = dict;

                item.ClickDisabled = Configuration.ClickDisableOn(x);

                return item;
            })
            .AsEnumerable();
    }

    private string GetFormattedValue(Column<T, TKey> column, T obj)
    {
        var value = column.ValueGetter(obj);

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

    public TableResult ConstructResult(QueryModel query)
    {
        var result = new TableResult()
        {
            Items = GetDisplayData(query).ToList()
        };

        if (query.IsPaginationNeeded())
        {
            result.AllItemCount = GetAllDataCount();
        }

        if (query.IsTextFilterNeeded())
        {
            result.FilteredAllItemCount = GetAllFilteredCount(query);
        }

        return result;
    }
}
