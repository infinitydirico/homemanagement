using HomeManagement.Api.Identity.Filters;
using HomeManagement.API.Storage.Services;
using HomeManagement.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace HomeManagement.API.Storage
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddMvc(options =>
            {
                options.Filters.Add(new ExceptionFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddControllers();
            
            services.AddSingleton<IHostedService, TemporaryFilesCleanerService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            CreateTemporaryFolder();
        }

        private void CreateTemporaryFolder()
        {
            var directory = $@"{Directory.GetCurrentDirectory()}{String.GetOsSlash()}temporary";

            if (Directory.Exists(directory)) return;

            Directory.CreateDirectory(directory);
        }
    }
}
