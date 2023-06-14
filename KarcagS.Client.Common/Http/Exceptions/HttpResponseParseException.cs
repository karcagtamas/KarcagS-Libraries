namespace KarcagS.Client.Common.Http.Exceptions;

public class HttpResponseParseException : HttpException
{
    public HttpResponseParseException(string message, Exception e) : base(message, e)
    {
    }
}