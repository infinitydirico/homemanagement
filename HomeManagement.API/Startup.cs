using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeManagement.API.Data;
using HomeManagement.API.Data.Entities;
using HomeManagement.API.Data.Repositories;
using HomeManagement.API.Filters;
using HomeManagement.API.Throttle;
using HomeManagement.Contracts.Repositories;
using HomeManagement.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace HomeManagement.API
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
            services.AddDbContext<WebAppDbContext>(options =>
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

            AddRepositories(services);
            AddMiddleware(services);

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ThrottleFilter));
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Idnetity API", Version = "v1" });
            });

            // ********************
            // Setup CORS
            // ********************
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin(); // For anyone access.
            //corsBuilder.WithOrigins("http://localhost:56573"); // for a specific url. Don't add a forward slash on the end!
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

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

        private void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IPlatformContext, WebAppLayerContext>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ITokenRepository, TokenRepository>();

            services.AddScoped<IWebClientRepository, WebClientRepository>();
            
            //with the throttle filter with persisted repo, the requests take around 100ms to respond
            //with memory values, it takes 30ms
            //services.AddScoped<IWebClientRepository, MemoryWebClientRepository>();            
        }

        private void AddMiddleware(IServiceCollection services)
        {
            services.AddScoped<IThrottleCore, ThrottleCore>();
        }
    }
}
