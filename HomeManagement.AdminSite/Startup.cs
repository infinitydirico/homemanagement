using HomeManagement.AdminSite.Data;
using HomeManagement.AdminSite.Services;
using HomeManagement.Api.Core.HealthChecks;
using HomeManagement.Contracts;
using HomeManagement.Core.Cryptography;
using HomeManagement.Data;
using HomeManagement.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HomeManagement.AdminSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var postgresConnection = Configuration.GetSection("ConnectionStrings").GetValue<string>("Postgres");
            services.AddDbContextPool<AdminDbContext>(options =>
                options.UseNpgsql(postgresConnection));

            services.AddLogging();

            services.AddScoped<IAuthenticationService, AuthenticationApiService>();
            services.AddScoped<ICryptography, AesCryptographyService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IPlatformContext, AdminAppLayerContext>();
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();

            services.AddScoped<ICategoryMapper, CategoryMapper>();
            services.AddScoped<ITransactionMapper, TransactionMapper>();
            services.AddScoped<IConfigurationSettingsMapper, ConfigurationSettingsMapper>();
            services.AddScoped<Business.Contracts.IExportableCategory, Business.Exportation.ExportableCategory>();
            services.AddScoped<Business.Contracts.IUserSessionService, UserSessionService>();
            services.AddScoped<Business.Contracts.ICategoryService, Business.Units.CategoryService>();
            services.AddScoped<Business.Contracts.IConfigurationSettingsService, Business.Units.ConfigurationSettingsService>();
            services.AddScoped<IEndpointsHealthService, EndpointsHealthService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHealthChecks()
                .AddCheck<MemoryHealthCheck>("memory");
                //.AddCheck("ping", new PingHealthCheck(Configuration.GetSection("Endpoints").GetValue<string>("Identity"), 100));

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Home/Error");

            loggerFactory.AddFile("logs/logfile-{Date}.txt");

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
