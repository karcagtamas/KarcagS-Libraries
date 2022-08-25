namespace KarcagS.Common.Tools.Table;

public class TableException : Exception
{
    public TableException()
    {

    }

    public TableException(string msg) : base(msg)
    {

    }

    public TableException(string msg, Exception exception) : base(msg, exception)
    {

    }
}
