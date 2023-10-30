using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using WebApiCore.AspNetCore.Middleware.AccessTokenAuthentication;

namespace WebApiCore.AspNetCore.Middleware.AccessTokenAuthentication.ConfigurationTokenProvider
{
    public static class ConfigurationTokenProviderExtension
    {
        /// <summary>
        /// Configure services required for <see cref="AccessTokenAuthenticationMiddleware"/> to work using <see cref="ConfigurationTokenProvider"/>
        /// </summary>
        /// <param name="tokenKey">Key of access token for GET or POST request</param>
        public static AccessTokenAuthenticationConfiguration AddConfigurationAccessTokenAuthentication(this IServiceCollection services, string tokenKey)
        {
            services.AddSingleton<ConfigurationTokenProvider>();
            var cfg = services.AddAccessTokenAuthentication(s => s.GetRequiredService<ConfigurationTokenProvider>(), tokenKey);
            return cfg;
        }

        /// <summary>
        /// Configure services required for <see cref="AccessTokenAuthenticationMiddleware"/> to work using <see cref="ConfigurationTokenProvider"/> and default access token key for requests
        /// </summary>
        public static AccessTokenAuthenticationConfiguration AddConfigurationAccessTokenAuthentication(this IServiceCollection services)
        {
            services.AddSingleton<ConfigurationTokenProvider>();
            var cfg = services.AddAccessTokenAuthentication(s => s.GetRequiredService<ConfigurationTokenProvider>());
            return cfg;
        }
    }
}
