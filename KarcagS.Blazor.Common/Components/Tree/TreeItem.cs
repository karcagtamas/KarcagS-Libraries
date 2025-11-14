using MudBlazor;

namespace KarcagS.Blazor.Common.Components.Tree;

public sealed class TreeItem<T> : TreeItemData<T>
{
    public T Data { get; set; }
    public int Level { get; set; }
    public bool HasChild => Children?.Count > 0;
    public bool IsLeaf => Children?.Count == 0;

    public TreeItem(T data, Func<T, List<T>> itemExtractor, bool isExpanded = true)
    {
        Data = data;
        Level = 0;
        Expanded = isExpanded;
        Children = itemExtractor(data).Select(x => new TreeItem<T>(x, itemExtractor, Level + 1, isExpanded)).Select(TreeItemData<T> (item) => item).ToList();
    }

    private TreeItem(T data, Func<T, List<T>> itemExtractor, int level, bool isExpanded = true)
    {
        Data = data;
        Level = level;
        Expanded = isExpanded;
        Children = itemExtractor(data).Select(x => new TreeItem<T>(x, itemExtractor, Level + 1)).Select(TreeItemData<T> (item) => item).ToList();
    }

    public void Expand() => Expanded = true;

    public void ExpandAll()
    {
        Expand();
        Children?.Select(item => item as TreeItem<T>).ToList().ForEach(x => x?.ExpandAll());
    }

    public void Collapse() => Expanded = false;

    public void CollapseAll()
    {
        Collapse();
        Children?.Select(item => item as TreeItem<T>).ToList().ForEach(x => x?.CollapseAll());
    }

    public void ToggleSelection(bool toggleAll = false)
    {
        Selected = !Selected;

        if (toggleAll)
        {
            Children?.Select(item => item as TreeItem<T>).ToList().ForEach(x => x?.ToggleSelection(true));
        }
    }

    public static IReadOnlyCollection<TreeItem<T>> ConvertList(List<T> list, Func<T, List<T>> itemExtractor, bool isExpanded = true) => list.Select(x => new TreeItem<T>(x, itemExtractor, isExpanded)).ToHashSet();
}