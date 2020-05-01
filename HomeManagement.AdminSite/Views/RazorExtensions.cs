using Microsoft.AspNetCore.Http;

namespace HomeManagement.AdminSite.Views
{
    public static class RazorExtensions
    {
        public static string GetHeader(this HttpContext context, string header)
            => context.Request.Headers.ContainsKey(header) ? context.Request.Headers[header].ToString() : string.Empty;

        public static HostString GetHost(this HttpContext context)
        {
            var host = context.GetHeader("X-Forwarded-Host");

            if (string.IsNullOrEmpty(host)) return context.Request.Host;
            else
            {
                var pathbase = context.Request.Headers["X-Forwarded-PathBase"];
                var proxy = host + pathbase;
                return new HostString(proxy);
            }
        }

        public static bool ContainsForwardedHeaders(this HttpContext context) 
            => !string.IsNullOrEmpty(context.GetHeader("X-Forwarded-Host"));

        public static string GetScheme(this HttpContext context) => context.Request.Scheme;
    }
}
