using Domain.Utils.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace Domain.Utils.Helpers
{
    public static class HttpContextHelper
    {
        private static JwtSecurityToken? HeaderAuthToken { get; set; }

        public static int GetUserId()
            => HeaderAuthToken?.GetClaimValue(HeaderKey.NameIdentifier)?.ToInt("InvalidAuthToken")
                ?? throw new InvalidOperationException("ErrorGettingSessionInfo");

        public static void SaveTokens(this IHttpContextAccessor contextAccessor)
        {
            HeaderAuthToken = contextAccessor.GetAuthTokenByHeader();
        }

        private static JwtSecurityToken? GetAuthTokenByHeader(this IHttpContextAccessor contextAccessor)
        {
            try
            {
                contextAccessor.HttpContext!.Request.Headers.TryGetValue(HeaderKey.Authorization, out StringValues headerValue);

                var stringToken = headerValue.FirstOrDefault()?.ToCleanJwtToken()
                    ?? throw new InvalidOperationException("ErrorGettingSessionInfo");

                return new JwtSecurityTokenHandler().ReadJwtToken(stringToken);
            }
            catch
            {
                return null;
            }
        }
    }
}
