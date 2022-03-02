using KarcagS.Shared.Http;
using Microsoft.AspNetCore.Mvc;

namespace KarcagS.Common.Tools.HttpInterceptor;

public class ValidationErrorActionResult : IActionResult
{
    public const int ValidationErrorCode = 499;

    private readonly HttpResult<object> result;

    public ValidationErrorActionResult(HttpResult<object> result)
    {
        this.result = result;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        await new ObjectResult(result)
        {
            StatusCode = ValidationErrorCode
        }.ExecuteResultAsync(context);
    }
}
