using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

        public async Task InvokeAsync(HttpContext context)
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
                var user = await _tokenProvider.GetTokenUserAsync(token)
                    .ConfigureAwait(false);

                if (context.User != null)
                {
                    if (user != null)
                    {
                        List<ClaimsIdentity> identities = new List<ClaimsIdentity>(context.User.Identities);
                        identities.AddRange(user.Identities);

                        context.User = new ClaimsPrincipal(identities);
                    }
                }
                else
                    context.User = user;
            }

            await _next(context);
        }
    }
}
