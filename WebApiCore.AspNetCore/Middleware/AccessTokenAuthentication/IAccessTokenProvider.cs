using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace WebApiCore.AspNetCore.Middleware.AccessTokenAuthentication
{
    public interface IAccessTokenProvider
    {
        bool TokenExists(string accessToken);

        ClaimsPrincipal GetTokenUser(string accessToken);
    }
}
