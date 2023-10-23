using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiCore.AspNetCore.Middleware.AccessTokenAuthentication
{
    public static class AccessTokenAuthenticationMiddlewareExtension
    {
        /// <summary>
        /// Configure services required for <see cref="AccessTokenAuthenticationMiddleware"/> to work
        /// </summary>
        /// <typeparam name="TTokenProvider">Service that will return User for HttpContext by provided token.</typeparam>
        /// <param name="tokenProvider">Configured token provider instance</param>
        /// <param name="tokenKey">Key of access token in GET or POST request</param>
        public static AccessTokenAuthenticationConfiguration AddAccessTokenAuthentication<TTokenProvider>(this IServiceCollection services, TTokenProvider tokenProvider, string tokenKey)
            where TTokenProvider : class, IAccessTokenProvider
        {
            services.AddSingleton(tokenProvider);

            var cfg = new AccessTokenAuthenticationConfiguration()
            {
                TokenKey = tokenKey
            };
            services.AddSingleton(cfg);

            return cfg;
        }

        /// <summary>
        /// Configure services required for <see cref="AccessTokenAuthenticationMiddleware"/> to work
        /// </summary>
        /// <typeparam name="TTokenProvider">Service that will return User for HttpContext by provided token.</typeparam>
        /// <param name="tokenKey">Key of access token in GET or POST request</param>
        public static AccessTokenAuthenticationConfiguration AddAccessTokenAuthentication<TTokenProvider>(this IServiceCollection services, string tokenKey)
            where TTokenProvider : class, IAccessTokenProvider, new()
        {
            services.AddSingleton<TTokenProvider>();
            var cfg = new AccessTokenAuthenticationConfiguration();
            services.AddSingleton(cfg);
            cfg.TokenKey = tokenKey;

            return cfg;
        }

        public static AccessTokenAuthenticationConfiguration AddAccessTokenAuthentication<TTokenProvider>(this IServiceCollection services, TTokenProvider tokenProvider)
            where TTokenProvider : class, IAccessTokenProvider
        {
            return services.AddAccessTokenAuthentication(tokenProvider, AccessTokenAuthenticationConfiguration.DEFAULT_TOKEN_KEY);
        }

        public static AccessTokenAuthenticationConfiguration AddAccessTokenAuthentication<TTokenProvider>(this IServiceCollection services)
            where TTokenProvider : class, IAccessTokenProvider, new()
        {
            return services.AddAccessTokenAuthentication<TTokenProvider>(AccessTokenAuthenticationConfiguration.DEFAULT_TOKEN_KEY);
        }

        public static IApplicationBuilder UseAccessTokenAuthentication(this IApplicationBuilder app)
        {
            app.UseMiddleware<AccessTokenAuthenticationMiddleware>();

            return app;
        }
    }
}
