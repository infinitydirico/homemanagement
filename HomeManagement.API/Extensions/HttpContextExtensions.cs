using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace HomeManagement.API.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetHeader(this HttpContext httpContext, string key) => httpContext?.Request?.Headers?[key].FirstOrDefault();

        public static string GetAuthorizationHeader(this HttpContext httpContext) => httpContext?.Request?.Headers?["Authorization"].FirstOrDefault();

        public static Claim GetEmailClaim(this HttpContext httpContext) => httpContext?.User?.Claims?.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub));
    }
}
