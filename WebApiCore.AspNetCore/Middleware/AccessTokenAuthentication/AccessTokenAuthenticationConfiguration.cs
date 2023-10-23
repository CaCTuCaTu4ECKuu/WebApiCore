namespace WebApiCore.AspNetCore.Middleware.AccessTokenAuthentication
{
    public class AccessTokenAuthenticationConfiguration
    {
        public const string DEFAULT_TOKEN_KEY = "access_token";

        /// <summary>
        /// Key of access token in GET or POST request
        /// </summary>
        public string TokenKey { get; set; } = DEFAULT_TOKEN_KEY;
    }
}
