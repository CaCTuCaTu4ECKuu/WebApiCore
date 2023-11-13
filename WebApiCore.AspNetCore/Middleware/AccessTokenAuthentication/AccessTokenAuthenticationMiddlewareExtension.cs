using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApiCore.AspNetCore.Middleware.AccessTokenAuthentication;
using WebApiCore.AspNetCore.Middleware.OperationResultExceptionJsonWrapper;

namespace WebApiCore.AspNetCore
{
    public static class AccessTokenAuthenticationMiddlewareExtension
    {
        /// <summary>
        /// Extract and return token key if it was provided within the request 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tokenKey">Key to search Access Token</param>
        /// <returns>Access Token or null of key was not provided</returns>
        public static string GetAccessToken(this HttpContext context, string tokenKey = "access_token")
        {
            string token = null;
            if (context.Request.Method == "GET")
            {
                if (context.Request.Query.ContainsKey(tokenKey))
                {
                    if (context.Request.Query.TryGetValue(tokenKey, out var extractedApiKey))
                        token = extractedApiKey.ToString();
                }
            }
            else
            {
                if (context.Request.Headers.ContainsKey(tokenKey))
                {
                    if (context.Request.Headers.TryGetValue(tokenKey, out var extractedApiKey))
                        token = extractedApiKey.ToString();
                }
            }

            return token;
        }

        public static OperationResultExceptionJsonWrapperConfiguration SetAccessTokenExceptionsWrapper(this OperationResultExceptionJsonWrapperConfiguration config)
        {
            config.SetExceptionWrapper<AccessTokenException>();
            return config;
        }

        /// <summary>
        /// Configure services required for <see cref="AccessTokenAuthenticationMiddleware"/> to work
        /// </summary>
        /// <param name="tokenProvider">Configured token provider</param>
        /// <param name="tokenKey">Key of access token for GET or POST request</param>
        public static AccessTokenAuthenticationConfiguration AddAccessTokenAuthentication<TTokenProvider>(this IServiceCollection services, Func<IServiceProvider, TTokenProvider> tokenProvider, string tokenKey)
            where TTokenProvider : class, IAccessTokenProvider
        {
            services.TryAddSingleton<IAccessTokenProvider>(tokenProvider);

            var cfg = new AccessTokenAuthenticationConfiguration()
            {
                TokenKey = tokenKey
            };
            services.AddSingleton(cfg);

            return cfg;
        }

        /// <summary>
        /// Configure services required for <see cref="AccessTokenAuthenticationMiddleware"/> to work using default access token key for requests
        /// </summary>
        /// <param name="tokenProvider">Configured token provider</param>
        public static AccessTokenAuthenticationConfiguration AddAccessTokenAuthentication<TTokenProvider>(this IServiceCollection services, Func<IServiceProvider, TTokenProvider> tokenProvider)
            where TTokenProvider : class, IAccessTokenProvider
        {
            return services.AddAccessTokenAuthentication(tokenProvider, AccessTokenAuthenticationConfiguration.DEFAULT_TOKEN_KEY);
        }

        public static IApplicationBuilder UseAccessTokenAuthentication(this IApplicationBuilder app)
        {
            app.UseMiddleware<AccessTokenAuthenticationMiddleware>();

            return app;
        }
    }
}
