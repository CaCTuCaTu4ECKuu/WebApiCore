using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

using WebApiCore.Exceptions;

namespace WebApiCore.AspNetCore.ActionFilters
{
    /// <summary>
    /// Filter that ensure the avaliability of access token and return error if not provided. Put provided token into context items collection under "access_token" key
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireAccessTokenAttribute : Attribute, IAsyncActionFilter
    {
        public string TokenKey { get; set; } = "access_token";

        public RequireAccessTokenAttribute()
        {

        }

        public RequireAccessTokenAttribute(string tokenKey)
        {
            TokenKey = tokenKey;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!HasAccessToken(context, out var extractedApiKey) || string.IsNullOrWhiteSpace(extractedApiKey))
            {
                var apiErrors = context.HttpContext.RequestServices.GetRequiredService<IApiErrorCodes>();
                context.Result = new ContentResult()
                {
                    StatusCode = apiErrors.GetHTTPResponseCode(BasicApiErrorCodes.ACCESS_TOKEN_INVALID),
                    Content = "Access Token was not provided"
                };
                return;
            }

            context.HttpContext.Items["access_token"] = extractedApiKey.ToString();

            await next();
        }

        private bool HasAccessToken(ActionExecutingContext context, out StringValues extractedApiKey)
        {
            if (context.HttpContext.Request.Method == "GET")
            {
                return context.HttpContext.Request.Query.TryGetValue(TokenKey, out extractedApiKey);
            }
            else
            {
                return context.HttpContext.Request.Headers.TryGetValue(TokenKey, out extractedApiKey);
            }
        }
    }
}
