using Microsoft.AspNetCore.Http;

namespace WebApiCore.AspNetCore
{
    public static class AccessTokenHttpContextExtension
    {
        /// <summary>
        /// Returns 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetAccessToken(this HttpContext context)
        {
            if (context.Items.ContainsKey("access_token"))
                return (string)context.Items["access_token"];

            return null;
        }
    }
}
