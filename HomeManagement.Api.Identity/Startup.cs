using HomeManagement.Api.Identity.Data;
using HomeManagement.Api.Identity.Filters;
using HomeManagement.Contracts;
using HomeManagement.Core.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
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
                var corsBuilder = new CorsPolicyBuilder();
                corsBuilder.AllowAnyHeader();
                corsBuilder.AllowAnyMethod();
                corsBuilder.AllowAnyOrigin();
                corsBuilder.AllowCredentials();
                options.AddPolicy("IdentityApiCorsPolicy", corsBuilder.Build());
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(new ExceptionFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ICryptography, AesCryptographyService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Idnetity API V1");
            });

            app.UseCors("IdentityApiCorsPolicy");

            app.UseMvc();

            CreateDatabseIfNotExits(app);
        }

        private void CreateDatabseIfNotExits(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices
                                        .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<WebIdentityDbContext>();

                var pendingMigrations = context.Database.GetPendingMigrations();
                context.Database.Migrate();

                context.SeedRoles();
            }
        }
    }
}
