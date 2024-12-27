using KarcagS.Blazor.Common.Components.Table.Styles;
using KarcagS.Shared.Table;

namespace KarcagS.Blazor.Common.Components.Table;

public class TableRowItem<TKey> : ResultItem<TKey>
{
    public bool Selected { get; set; } = false;
    public bool Disabled { get; set; } = false;
    public Dictionary<string, TableDataStyle<TKey>> Styles { get; set; } = new();

    public TableRowItem()
    {
    }

    public TableRowItem(ResultItem<TKey> item, StyleConfiguration<TKey> styleConfiguration, TableMetaData metaData)
    {
        ItemKey = item.ItemKey;
        Values = item.Values;
        Tags = item.Tags;
        ClickDisabled = item.ClickDisabled;

        var styles = new Dictionary<string, TableDataStyle<TKey>>();
        Values.Keys.ToList().ForEach(key =>
        {
            var cellStyle = styleConfiguration.CellStyleGetter(key, Values[key]);
            styles.Add(key,
                new TableDataStyle<TKey>(cellStyle,
                    cellStyle.GetClass(styleConfiguration, metaData.ColumnsData.Columns.First(col => col.Key == key), this),
                    cellStyle.GetStyle(styleConfiguration.ColumnStyleGetter(key))));
        });
        Styles = styles;
    }
}