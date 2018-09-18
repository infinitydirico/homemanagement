using Microsoft.AspNetCore.Http;
using System.Linq;

namespace HomeManagement.API.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetAuthorizationHeader(this HttpContext httpContext) => httpContext?.Request?.Headers?["Authorization"].FirstOrDefault();
    }
}
