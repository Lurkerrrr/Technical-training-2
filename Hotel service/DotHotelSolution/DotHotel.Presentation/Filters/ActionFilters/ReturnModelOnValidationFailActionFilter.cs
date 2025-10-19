using System.Reflection;
using DotHotel.Presentation.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotHotel.Presentation.Filters.ActionFilters
{
    public class ReturnModelOnValidationFailActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid && context.Controller is Controller controller)
            {
                if (context.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
                {
                    var methodInfo = actionDescriptor.MethodInfo;
                    var parameters = methodInfo.GetParameters();

                    var markedParameter = parameters
                        .FirstOrDefault(p => p.GetCustomAttribute<ReturnOnValidationFailAttribute>() != null);

                    if (markedParameter != null &&
                        context.ActionArguments.TryGetValue(markedParameter.Name!, out var model))
                    {
                        context.Result = controller.View(model);
                        return;
                    }
                }

                context.Result = controller.View();
                return;
            }

            await next();
        }
    }
}
