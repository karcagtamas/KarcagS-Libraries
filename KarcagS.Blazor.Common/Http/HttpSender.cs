using Karcags.Common.Tools.ErrorHandling;

namespace KarcagS.Blazor.Common.Http;

public class HttpSender<T>
{
    private readonly List<Action<T?>> successActions = new();
    private readonly List<Action<HttpResultError?>> errorActions = new();
    private readonly Func<HttpResult<T>?> sender;

    public HttpSender(Func<HttpResult<T>?> sender)
    {
        this.sender = sender;
    }

    public HttpSender<T> Success(Action<T?> success)
    {
        successActions.Add(success);
        return this;
    }

    public HttpSender<T> Error(Action<HttpResultError?> error)
    {
        errorActions.Add(error);
        return this;
    }

    public void Perform()
    {
        var response = sender();

        if (response is not null)
        {
            if (response.IsSuccess)
            {
                successActions.ForEach(a => a(response.Result));
            }
            else
            {
                errorActions.ForEach(a => a(response.Error));
            }
        }
        else
        {
            errorActions.ForEach(a => a(null));
        }
    }
}
