using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace WebApiCore.AspNetCore.Middleware.AccessTokenAuthentication
{
    public class ConfigurationTokenProvider : IAccessTokenProvider
    {
        public const string CONFIGURATION_KEY = "AccessTokens";

        private readonly IConfiguration _config;

        public ConfigurationTokenProvider(IConfiguration config)
        {
            _config = config;
        }

        public ClaimsPrincipal GetTokenUser(string accessToken)
        {
            var token = _config.GetSection($"{CONFIGURATION_KEY}:{accessToken}");
            if (token.Exists())
            {
                var claims = new List<Claim>();

                var uName = _config[$"{CONFIGURATION_KEY}:{accessToken}:NameAlias"];
                if (!string.IsNullOrEmpty(uName))
                    claims.Add(new Claim(ClaimTypes.Name, uName));

                var rolesValue = _config[$"{CONFIGURATION_KEY}:{accessToken}:Roles"];
                if (!string.IsNullOrEmpty(rolesValue))
                {
                    var roles = rolesValue.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var role in roles)
                        claims.Add(new Claim(ClaimTypes.Role, role.Trim().ToUpper()));
                }

                var identity = new ClaimsIdentity(claims, "Basic");
                return new ClaimsPrincipal(identity);
            }

            return null;
        }

        public bool TokenExists(string accessToken)
        {
            return _config.GetSection($"{CONFIGURATION_KEY}:{accessToken}").Exists();
        }
    }
}
