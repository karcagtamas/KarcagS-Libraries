using KarcagS.Shared.Http;

namespace KarcagS.Blazor.Common.Http;

public class HttpSender<T>
{
    private readonly List<Action<T?>> successActions = new();
    private readonly List<Action<HttpResultError?>> errorActions = new();
    private readonly Func<Task<HttpResult<T>?>> sender;

    public HttpSender(Func<Task<HttpResult<T>?>> sender)
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

    public async Task<bool> Execute() => (await Perform())?.IsSuccess ?? false;
    

    public async Task<T?> ExecuteWithResult()
    {
        var res = await Perform();

        if (res is null)
        {
            return default;
        }

        return res.Result;
    }

    private async Task<HttpResult<T>?> Perform()
    {
        var response = await sender();

        if (response is not null)
        {
            if (response.IsSuccess)
            {
                successActions.ForEach(a => a(response.Result));
                return response;
            }
            else
            {
                errorActions.ForEach(a => a(response.Error));
                return response;
            }
        }
        else
        {
            errorActions.ForEach(a => a(null));
            return null;
        }
    }
}
