using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebApiCore.AspNetCore.Middleware.AccessTokenAuthentication.ConfigurationTokenProvider
{
    public class ConfigurationTokenProvider : IAccessTokenProvider
    {
        public const string CONFIGURATION_KEY = "AccessTokens";

        private readonly IConfiguration _config;

        public string AuthenticationScheme { get; private set; } = "ConfigurationToken";

        public ConfigurationTokenProvider(IConfiguration config)
        {
            _config = config;
        }
        public ConfigurationTokenProvider(IConfiguration config, string authenticationScheme)
        {
            _config = config;
            if (string.IsNullOrEmpty(authenticationScheme))
                throw new ArgumentException("Authentication type is not defined", nameof(authenticationScheme));

            AuthenticationScheme = authenticationScheme;
        }

        public ClaimsPrincipal GetTokenUser(string accessToken)
        {
            if (TokenExists(accessToken))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, accessToken)
                };

                var uName = _config[$"{CONFIGURATION_KEY}:{accessToken}:NameAlias"];
                if (!string.IsNullOrEmpty(uName))
                    claims.Add(new Claim(ClaimTypes.Name, uName));

                var rolesValue = _config[$"{CONFIGURATION_KEY}:{accessToken}:Roles"];
                if (!string.IsNullOrEmpty(rolesValue))
                {
                    var roles = rolesValue.Split(',');
                    foreach (var role in roles)
                        claims.Add(new Claim(ClaimTypes.Role, role.Trim().ToUpper()));
                }

                var identity = new ClaimsIdentity(claims, AuthenticationScheme);
                return new ClaimsPrincipal(identity);
            }

            return null;
        }

        public bool TokenExists(string accessToken)
        {
            var tokens = _config.GetSection(CONFIGURATION_KEY);
            if (tokens.Exists())
                return tokens.AsEnumerable().Any(e => e.Key == $"{CONFIGURATION_KEY}:{accessToken}");

            return false;
        }

        public Task<ClaimsPrincipal> GetTokenUserAsync(string accessToken)
        {
            return Task.FromResult(GetTokenUser(accessToken));
        }
    }
}
