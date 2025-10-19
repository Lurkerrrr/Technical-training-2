using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotHotel.Presentation.Filters.ActionFilters
{
    public class PassParametersToViewBagActionFilter : IAsyncActionFilter
    {
        private readonly string[] _parameterNames;

        public PassParametersToViewBagActionFilter(string[] parameterNames)
        {
            _parameterNames = parameterNames;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();

            if (executedContext.Result is ViewResult viewResult)
            {
                foreach (var paramName in _parameterNames)
                {
                    if (context.ActionArguments.TryGetValue(paramName, out var value))
                    {
                        var viewBagKey = char.ToUpper(paramName[0]) + paramName.Substring(1);
                        viewResult.ViewData[viewBagKey] = value;
                    }
                }
            }
        }
    }
}
