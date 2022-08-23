using KarcagS.Shared.Table;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KarcagS.Common.Attributes;

public class ExtraParamsActionFilter : ActionFilterAttribute
{
    private const string QueryFieldName = "query";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var extraParams = new Dictionary<string, object>();

        if (context.ActionArguments[QueryFieldName] is QueryModel model)
        {
            foreach (var kvp in context.HttpContext.Request.Query)
            {
                if (!context.ActionArguments.ContainsKey(kvp.Key))
                {
                    extraParams.Add(kvp.Key, kvp.Value);
                }
            }
            model.ExtraParams = extraParams;
            context.ActionArguments[QueryFieldName] = model;
            base.OnActionExecuting(context);
        }
    }
}
