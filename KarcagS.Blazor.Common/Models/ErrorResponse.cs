namespace KarcagS.Blazor.Common.Models;

/// <summary>
/// Error Response
/// </summary>
public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public ErrorResponse? Inner { get; set; }
    public string? StackTrace { get; set; }

    // For deserialization
    public ErrorResponse()
    {

    }

    public ErrorResponse(Exception e)
    {
        Message = e.Message;

        // Inner exception
        if (e.InnerException is not null)
        {
            Inner = new ErrorResponse(e.InnerException);
        }

        // Stack trace
        if (e.StackTrace is not null)
        {
            StackTrace = e.StackTrace;
        }
    }
}
