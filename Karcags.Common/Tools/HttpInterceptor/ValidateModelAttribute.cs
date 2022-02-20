using KarcagS.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace KarcagS.Common.Tools.HttpInterceptor;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(new HttpResult<object>
            {
                IsSuccess = false,
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Error = new HttpResultError
                {
                    Message = "Validation error",
                    SubMessages = context.ModelState
                        .SelectMany(x =>
                        {
                            if (x.Value is null)
                            {
                                return new List<string>();
                            }

                            return x.Value.Errors.Select(e =>
                            {
                                return $"{x.Key}: {e.ErrorMessage}";
                            }).ToList();
                        })
                        .ToArray()
                }
            });
        }

        base.OnActionExecuting(context);
    }
}
