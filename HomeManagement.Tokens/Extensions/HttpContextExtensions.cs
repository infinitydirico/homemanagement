using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace HomeManagement.Api.Core.Extensions
{
    public static class HttpContextExtensions
    {
        public static bool HasHeader(this HttpContext httpContext, string key) => httpContext.Request.Headers.ContainsKey(key);

        public static string GetHeader(this HttpContext httpContext, string key) => httpContext?.Request?.Headers?[key].FirstOrDefault();

        public static string GetAuthorizationHeader(this HttpContext httpContext) => httpContext?.Request?.Headers?["Authorization"].FirstOrDefault();

        public static string GetMobileHeader(this HttpContext httpContext) => httpContext?.Request?.Headers?["HomeManagementApp"].FirstOrDefault();

        public static Claim GetEmailClaim(this HttpContext httpContext) => httpContext?.User?.Claims?.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub));

        public static string GetEmail(this HttpContext httpContext) => GetEmailClaim(httpContext).Value;
    }
}
