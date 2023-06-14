namespace KarcagS.Client.Common.Http.Exceptions;

public class HttpException : Exception
{
    public HttpException()
    {
    }

    public HttpException(string message) : base(message)
    {
    }

    public HttpException(string message, Exception e) : base(message, e)
    {
    }
}