using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApiCore.AspNetCore.Middleware.AccessTokenAuthentication
{
    public interface IAccessTokenProvider
    {
        /// <summary>
        /// Check if token exist
        /// </summary>
        /// <param name="accessToken"></param>
        bool TokenExists(string accessToken);

        ClaimsPrincipal GetTokenUser(string accessToken);

        Task<ClaimsPrincipal> GetTokenUserAsync(string accessToken);
    }
}
