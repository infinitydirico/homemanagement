using Microsoft.Extensions.DependencyInjection;

namespace HomeManagement.Api.Core.Throttle
{
    public static class ThrottleStartup
    {
        public static IServiceCollection AddThrottlingService(this IServiceCollection services)
        {
            services.AddScoped<IThrottleCore, ThrottleCore>();
            return services;
        }
    }
}
