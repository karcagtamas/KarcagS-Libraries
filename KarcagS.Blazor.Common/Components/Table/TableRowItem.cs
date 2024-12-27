using KarcagS.Blazor.Common.Components.Table.Styles;
using KarcagS.Shared.Table;

namespace KarcagS.Blazor.Common.Components.Table;

public class TableRowItem<TKey> : ResultItem<TKey>
{
    public bool Selected { get; set; } = false;
    public bool Disabled { get; set; } = false;
    public Dictionary<string, CellStyle> Styles { get; set; } = new();

    public TableRowItem()
    {
    }

    public TableRowItem(ResultItem<TKey> item) : this(item, (_, _) => CellStyleBuilder.Default())
    {
    }

    public TableRowItem(ResultItem<TKey> item, Func<string, ItemValue, CellStyle> cellStyleGetter)
    {
        ItemKey = item.ItemKey;
        Values = item.Values;
        Tags = item.Tags;
        ClickDisabled = item.ClickDisabled;

        var styles = new Dictionary<string, CellStyle>();
        Values.Keys.ToList().ForEach(key => { styles.Add(key, cellStyleGetter(key, Values[key])); });
        Styles = styles;
    }
}