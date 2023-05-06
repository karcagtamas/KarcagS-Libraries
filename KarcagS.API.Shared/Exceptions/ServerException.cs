namespace KarcagS.API.Shared.Exceptions;

/// <summary>
/// Own Generated Message Exception
/// </summary>
public class ServerException : Exception
{
    public string? ResourceKey { get; set; } = null;

    /// <summary>
    /// Empty init
    /// </summary>
    public ServerException()
    {
        ResourceKey = null;
    }

    /// <summary>
    /// Exception with message
    /// </summary>
    /// <param name="msg">Exception message</param>
    public ServerException(string msg, string? resourceKey = null) : base(msg)
    {
        ResourceKey = resourceKey;
    }

    /// <summary>
    /// Wrapping an exception into a Server Exception
    /// </summary>
    /// <param name="message">Message</param>
    /// <param name="innerException">Inner exception</param>
    public ServerException(string message, Exception innerException, string? resourceKey) : base(message, innerException)
    {
        ResourceKey = resourceKey;
    }
}
