using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Karcags.Common.Tools.ErrorHandling;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(new HttpResult
            {
                IsSuccess = false,
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Error = new ErrorResult
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
