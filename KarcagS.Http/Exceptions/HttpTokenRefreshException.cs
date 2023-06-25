namespace KarcagS.Http.Exceptions;

public class HttpTokenRefreshException : HttpException
{
    public HttpTokenRefreshException(string message) : base(message)
    {
    }

    public HttpTokenRefreshException(string message, Exception e) : base(message, e)
    {
    }
}