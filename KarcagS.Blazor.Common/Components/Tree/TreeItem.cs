namespace KarcagS.Blazor.Common.Components.Tree;

public class TreeItem<T>
{
    public T Data { get; set; } = default!;
    public bool IsExpanded { get; set; } = true;
    public bool IsSelected { get; set; } = false;
    public int Level { get; set; } = 0;
    public bool HasChild { get => Items.Count > 0; }
    public bool IsLeaf { get => Items.Count == 0; }
    public HashSet<TreeItem<T>> Items { get; set; } = new();

    public TreeItem(T data, Func<T, List<T>> itemExtractor, bool isExpanded = true)
    {
        Data = data;
        Level = 0;
        IsExpanded = isExpanded;
        Items = itemExtractor(data).Select(x => new TreeItem<T>(x, itemExtractor, Level + 1, isExpanded)).ToHashSet();
    }

    private TreeItem(T data, Func<T, List<T>> itemExtractor, int level, bool isExpanded = true)
    {
        Data = data;
        Level = level;
        IsExpanded = isExpanded;
        Items = itemExtractor(data).Select(x => new TreeItem<T>(x, itemExtractor, Level + 1)).ToHashSet();
    }

    public void Expand() => IsExpanded = true;

    public void ExpandAll()
    {
        Expand();
        Items.ToList().ForEach(x => x.ExpandAll());
    }

    public void Collapse() => IsExpanded = false;

    public void CollapseAll()
    {
        Collapse();
        Items.ToList().ForEach(x => x.CollapseAll());
    }

    public void ToggleSelection(bool toggleAll = false)
    {
        IsSelected = !IsSelected;

        if (toggleAll)
        {
            Items.ToList().ForEach(x => x.ToggleSelection(true));
        }
    }

    public static HashSet<TreeItem<T>> ConvertList(List<T> list, Func<T, List<T>> itemExtractor, bool isExpanded = true) => list.Select(x => new TreeItem<T>(x, itemExtractor, isExpanded)).ToHashSet();
}