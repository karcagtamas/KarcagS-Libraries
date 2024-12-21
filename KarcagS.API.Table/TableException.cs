namespace KarcagS.API.Table;

public class TableException : Exception
{
    public string? ResourceKey { get; set; }

    public TableException()
    {

    }

    public TableException(string msg) : base(msg)
    {

    }

    public TableException(string msg, string? resourceKey = null) : base(msg)
    {
        ResourceKey = resourceKey;
    }

    public TableException(string msg, Exception exception, string? resourceKey = null) : base(msg, exception)
    {
        ResourceKey = resourceKey;
    }
}
