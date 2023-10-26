using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApiCore.AspNetCore.Middleware.AccessTokenAuthentication;

namespace WebApiCore.AspNetCore
{
    public static class AccessTokenAuthenticationMiddlewareExtension
    {
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
