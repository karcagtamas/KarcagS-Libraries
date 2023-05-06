namespace KarcagS.API.Export;

/// <summary>
/// Table header object
/// </summary>
public class Header
{
    private string PropertyName { get; set; }
    public string DisplayName { get; set; }
    private Func<object, string> Displaying { get; set; }

    /// <summary>
    /// Header init
    /// </summary>
    /// <param name="property">Property name</param>
    /// <param name="display">Display name</param>
    /// <param name="displaying">Displaying method</param>
    public Header(string property, string display, Func<object, string> displaying)
    {
        PropertyName = property;
        DisplayName = display;
        Displaying = displaying;
    }

    /// <summary>
    /// Get string value from the given object
    /// </summary>
    /// <param name="obj">Source object</param>
    /// <typeparam name="T">Type of the object</typeparam>
    /// <returns>Current property's value</returns>
    public string GetValue<T>(T obj)
    {
        if (obj is null)
        {
            return string.Empty;
        }

        var type = obj.GetType();
        var property = type.GetProperty(this.PropertyName);

        return property is not null ? Displaying(property?.GetValue(obj) ?? string.Empty) : string.Empty;
    }
}
