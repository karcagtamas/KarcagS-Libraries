using System;

namespace Karcags.Common.Tools.ErrorHandling;

/// <summary>
/// Own Generated Message Exception
/// </summary>
public class ServerException : Exception
{
    /// <summary>
    /// Empty init
    /// </summary>
    public ServerException()
    {
    }

    /// <summary>
    /// Exception with message
    /// </summary>
    /// <param name="msg">Exception message</param>
    public ServerException(string msg) : base(msg)
    {
    }

    /// <summary>
    /// Wrapping an exception into a Server Exception
    /// </summary>
    /// <param name="message">Message</param>
    /// <param name="innerException">Inner exception</param>
    public ServerException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
