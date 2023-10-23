using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace WebApiCore.AspNetCore.Middleware.AccessTokenAuthentication
{
    public class AccessTokenAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AccessTokenAuthenticationConfiguration _ataConfig;
        private readonly IAccessTokenProvider _tokenProvider;

        public AccessTokenAuthenticationMiddleware(RequestDelegate next,
            AccessTokenAuthenticationConfiguration ataConfig,
            IAccessTokenProvider tokenProvider)
        {
            _next = next;
            _ataConfig = ataConfig;
            _tokenProvider = tokenProvider;
        }

        public void Invoke(HttpContext context)
        {
            string token = null;
            if (context.Request.Method == "GET")
            {
                if (context.Request.Query.TryGetValue(_ataConfig.TokenKey, out var extractedApiKey))
                    token = extractedApiKey.ToString();
            }
            else
            {
                if (context.Request.Headers.TryGetValue(_ataConfig.TokenKey, out var extractedApiKey))
                    token = extractedApiKey.ToString();
            }

            if (token != null && _tokenProvider.TokenExists(token))
            {
                context.User = _tokenProvider.GetTokenUser(token);
            }

            _next(context);
        }
    }
}
