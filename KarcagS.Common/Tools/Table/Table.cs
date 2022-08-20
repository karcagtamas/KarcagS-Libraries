using KarcagS.Common.Tools.Table.Configuration;
using KarcagS.Shared.Common;
using KarcagS.Shared.Table;
using static KarcagS.Shared.Table.TableResult;

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

    public TableMetaData<T, TKey> GetMetaData() => new(Configuration);

    public IEnumerable<ResultItem> GetDisplayData(QueryModel query)
    {
        return GetData(query)
            .Select(x =>
            {
                var item = new ResultItem
                {
                    ItemKey = x.Id?.ToString() ?? ""
                };

                var dict = new Dictionary<string, string>();

                Configuration.Columns.ForEach(col =>
                {
                    dict.Add(col.Key, GetFormattedValue(col, x));
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

    public TableResult ConstructResult(QueryModel query) => new()
    {
        Items = GetDisplayData(query).ToList(),
        AllItemCount = GetAllDataCount()
    };
}
