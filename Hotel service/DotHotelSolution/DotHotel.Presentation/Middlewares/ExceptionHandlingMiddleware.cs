using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DotHotel.Presentation.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                HandleException(context, ex);
            }
        }

        private void HandleException(HttpContext context, Exception exception)
        {
            if (context.Response.HasStarted)
            {
                return;
            }

            var tempDataProvider = context.RequestServices.GetService<ITempDataProvider>();
            var tempDataDictionary = tempDataProvider?.LoadTempData(context);
            if (tempDataProvider != null && tempDataDictionary != null)
            {
                tempDataDictionary["ErrorMessage"] = exception.Message;
                tempDataProvider.SaveTempData(context, tempDataDictionary);
            }

            string redirectUrl = GetRedirectUrl(context);
            context.Response.Redirect(redirectUrl);
        }

        private string GetRedirectUrl(HttpContext context)
        {
            var referer = context.Request.Headers["Referer"].FirstOrDefault();

            if (!string.IsNullOrEmpty(referer))
            {
                if (Uri.TryCreate(referer, UriKind.Absolute, out var refererUri))
                {
                    var requestHost = context.Request.Host.Host;
                    if (refererUri.Host.Equals(requestHost, StringComparison.OrdinalIgnoreCase))
                    {
                        return referer;
                    }
                }
            }

            return "/";
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
