using HomeManagement.Api.Identity.Data;
using HomeManagement.Api.Identity.Filters;
using HomeManagement.Api.Identity.Services;
using HomeManagement.Contracts;
using HomeManagement.Core.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeManagement.Api.Identity
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
            var postgresConnection = Configuration.GetSection("ConnectionStrings").GetValue<string>("Postgres");
            services.AddDbContextPool<WebIdentityDbContext>(options =>
                options.UseNpgsql(postgresConnection));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<WebIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddLogging();

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Idnetity API", Version = "v1" });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };

                c.AddSecurityDefinition("Bearer", securityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        securityScheme,
                        new List<string>()
                    }
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("IdentityApiCorsPolicy", corsBuilder =>
                    corsBuilder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(new ExceptionFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ICryptography, AesCryptographyService>();

            services.AddScoped<IBroadcaster, Broadcaster>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("logs/logfile-{Date}.txt");

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Idnetity API V1");
            });            

            app.UseRouting();

            app.UseAuthentication();

            app.UseCors("IdentityApiCorsPolicy");            

            app.UseEndpoints(x =>
            {
                x.MapDefaultControllerRoute().RequireCors("IdentityApiCorsPolicy");
            });

            CreateDatabseIfNotExits(app);
        }

        private void CreateDatabseIfNotExits(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices
                                        .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<WebIdentityDbContext>();

                var pendingMigrations = context.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    context.Database.Migrate();
                }

                context.SeedRoles();
            }
        }
    }
}
