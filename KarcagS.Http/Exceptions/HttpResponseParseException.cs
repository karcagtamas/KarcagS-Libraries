namespace KarcagS.Http.Exceptions;

public class HttpResponseParseException(string message, Exception e) : HttpException(message, e);