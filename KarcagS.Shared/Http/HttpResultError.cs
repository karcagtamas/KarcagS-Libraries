namespace Karcags.Common.Tools.ErrorHandling;

/// <summary>
/// Error response model
/// </summary>
public class HttpResultError
{
    public string Message { get; set; } = string.Empty;
    public string[] SubMessages { get; set; } = new string[0];
}
