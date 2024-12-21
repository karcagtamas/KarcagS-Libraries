using KarcagS.Shared.Helpers;
using KarcagS.Shared.Http;

namespace KarcagS.Http;

public class HttpSender<T>
{
    private readonly List<Action<T?>> successActions = [];
    private readonly List<Action<HttpErrorResult?>> errorActions = [];
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

    public HttpSender<T> Error(Action<HttpErrorResult?> error)
    {
        errorActions.Add(error);
        return this;
    }

    public async Task<bool> Execute() => (await Perform()).HttpResult?.IsSuccess ?? false;

    public async Task<SenderResult> ExecuteWithAll() => await Perform();

    public async Task<T?> ExecuteWithResult()
    {
        var res = await Perform();

        return ObjectHelper.IsNull(res.HttpResult)
            ? default
            : res.HttpResult.Result;
    }

    public async Task<T> ExecuteWithResultOrElse(T orElse)
    {
        var res = await Perform();

        if (ObjectHelper.IsNull(res.HttpResult))
        {
            return orElse;
        }

        return res.HttpResult.Result ?? orElse;
    }

    public async Task<ResultWrapper<T>> ExecuteWithWrapper()
    {
        var wrapper = new ResultWrapper<T>();

        var res = await Perform();

        if (ObjectHelper.IsNull(res.HttpResult))
        {
            return wrapper;
        }

        wrapper.Result = res.HttpResult.Result;
        wrapper.Error = res.HttpResult.Error;

        return wrapper;
    }

    private async Task<SenderResult> Perform()
    {
        HttpResult<T>? response;

        try
        {
            response = await sender();
        }
        catch (Exception e)
        {
            return SenderResult.OnlyException(e);
        }

        if (ObjectHelper.IsNotNull(response))
        {
            // Execute success actions
            if (response.IsSuccess)
            {
                successActions.ForEach(a => a(response.Result));
                return SenderResult.OnlyResult(response);
            }
        }

        // Response is not success or missing
        errorActions.ForEach(a => a(response?.Error));

        return ObjectHelper.IsNotNull(response)
            ? SenderResult.OnlyResult(response)
            : SenderResult.Empty();
    }

    public record SenderResult(HttpResult<T>? HttpResult, Exception? Exception)
    {
        public static SenderResult OnlyResult(HttpResult<T> httpResult) => new(httpResult, null);
        public static SenderResult OnlyException(Exception e) => new(null, e);
        public static SenderResult Empty() => new(null, null);
    }
}