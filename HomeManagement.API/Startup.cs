using System.Collections.Generic;
using System.Text;
using HomeManagement.API.Data;
using HomeManagement.API.Data.Entities;
using HomeManagement.API.Filters;
using HomeManagement.API.Infraestructure;
using HomeManagement.API.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using HomeManagement.API.Services;

namespace HomeManagement.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceCollection Services { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Services = services;

            services.AddLocalization(options => options.ResourcesPath = "Resource");

            services.AddDbContextPool<WebAppDbContext>(options =>
                options.UseSqlite("Data Source=HomeManagement.db"));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<WebAppDbContext>()
                .AddDefaultTokenProviders();


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(jwtBearerOptions =>
           {
               jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
               {
                   ValidateActor = false,
                   ValidateAudience = false,
                   ValidateLifetime = false,
                   ValidateIssuer = false,
                   ValidateIssuerSigningKey = false,
                   ValidIssuer = Configuration["Issuer"],
                   ValidAudience = Configuration["Audience"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SigningKey"]))
               };
           });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddRepositories();
            services.AddMiddleware();
            services.AddMappers();
            services.AddExportableComponents();

            services.AddScoped<ICurrencyService, CurrencyService>();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ThrottleFilter));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Idnetity API", Version = "v1" });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
            });

            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", options.BuildCorsPolicy());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new DatabaseLoggerProvider(app.ApplicationServices));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.EnsureDatabaseCreated();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRequestLocalization();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Idnetity API V1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
