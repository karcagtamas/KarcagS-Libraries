using System.Text;
using System.Text.Json;

namespace KarcagS.Http;

/// <summary>
/// HTTP body
/// </summary>
/// <typeparam name="T">Type of the content</typeparam>
public class HttpBody<T>
{
    private T Body { get; set; }

    /// <summary>
    /// Create body
    /// </summary>
    /// <param name="body">Content</param>
    public HttpBody(T body)
    {
        Body = body;
    }

    /// <summary>
    /// Create string content from the body
    /// </summary>
    /// <returns>String content</returns>
    public StringContent GetStringContent() => Body == null ? new StringContent("") : HttpBody<T>.CreateContent(Body);


    /// <summary>
    /// Create String Content
    /// </summary>
    /// <param name="obj">Object for creation</param>
    /// <returns>String content</returns>
    private static StringContent CreateContent(object obj) => new(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");

}
