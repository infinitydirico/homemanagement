using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProxyKit;
using System.Collections.Generic;

namespace HomeManagement.Proxy
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceCollection Services { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            Services = services;
            services.AddLogging();
            services.AddProxy();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("logs/logfile-{Date}.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("This is a proxy server.");
                });
            });

            var endpoints = GetEndpoints();

            foreach (var endpoint in endpoints)
            {
                var url = endpoint.GetSection("url");
                var mapping = endpoint.GetSection("mapping");
                app.Map(mapping.Value, server =>
                {
                    server.RunProxy(async context =>
                    {
                        var response = await context.ForwardTo(url.Value)
                                                    .AddXForwardedHeaders()
                                                    .Send();

                        return response;
                    });
                });
            }
        }

        public IEnumerable<IConfigurationSection> GetEndpoints()
        {
            var endpoints = Configuration.GetSection("Endpoints").GetChildren();

            foreach (var endpoint in endpoints)
            {
                yield return endpoint;
            }
        }
    }
}
