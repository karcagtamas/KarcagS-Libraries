using KarcagS.Shared.Http;
using Microsoft.AspNetCore.Mvc;

namespace KarcagS.API.Http.Interceptor;

public class ValidationErrorActionResult(HttpResult<object> result) : IActionResult
{
    public const int ValidationErrorCode = 499;

    public async Task ExecuteResultAsync(ActionContext context)
    {
        await new ObjectResult(result)
        {
            StatusCode = ValidationErrorCode
        }.ExecuteResultAsync(context);
    }
}
