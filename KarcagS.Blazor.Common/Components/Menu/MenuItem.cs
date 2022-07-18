using MudBlazor;

namespace KarcagS.Blazor.Common.Components.Menu;

public class MenuItem
{
    public string Title { get; set; }
    public string? Icon { get; set; }
    public Color? IconColor { get; set; }
    public string? Path { get; set; }
    public List<MenuItem> Children { get; set; }
    public bool IsDivider { get; set; } = false;
    public Action? Action { get; set; }
    public bool Authenticated { get; set; } = true;

    public bool IsGroup { get { return Children.Count > 0; } }
    public bool IsAction { get { return Path != null; } }

    public MenuItem(string title, string? icon, string? path, List<MenuItem> children)
    {
        Title = title;
        Icon = icon;
        Path = path;
        Children = children;
    }

    public static MenuItem CreateGroupItem(string title, List<MenuItem> children) => new(title, null, null, children);

    public static MenuItem CreateGroupItem(string title) => new(title, null, null, new List<MenuItem>());

    public static MenuItem CreateItem(string title, string path) => new(title, null, path, new List<MenuItem>());

    public static MenuItem CreateActionItem(string title, Action action)
    {
        MenuItem item = new(title, null, null, new List<MenuItem>())
        {
            Action = action
        };
        return item;
    }

    public static MenuItem CreateDivider()
    {
        MenuItem item = new("", null, null, new List<MenuItem>())
        {
            IsDivider = true
        };
        return item;
    }

    public MenuItem AddItem(MenuItem item)
    {
        Children.Add(item);
        return this;
    }

    public MenuItem AddIcon(string icon)
    {
        Icon = icon;
        return this;
    }

    public MenuItem AddIconColor(Color color)
    {
        IconColor = color;
        return this;
    }

    public MenuItem IsAuthenticated(bool authenticated)
    {
        Authenticated = authenticated;
        return this;
    }
}
