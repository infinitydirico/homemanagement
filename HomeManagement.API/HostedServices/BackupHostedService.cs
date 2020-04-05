using HomeManagement.API.Services;
using HomeManagement.Business.Contracts;
using HomeManagement.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace HomeManagement.API.HostedServices
{
    public class BackupHostedService : HostedService, IHostedService
    {
        private readonly IEmailService emailService;

        public BackupHostedService(ILogger<HostedService> logger, 
            IServiceScopeFactory factory,
            IEmailService emailService) 
            : base(logger, factory)
        {
            this.emailService = emailService;
        }

        //public override int GetPeriodToRun() => 60;

        public override int GetPeriodToRun() => 60 * 10;

        public override async void Process()
        {
            try
            {
                var context = GetService<IPlatformContext>();
                var configuration = GetService<IConfiguration>();
                var preferenceRepository = new PreferencesRepository(context.CreateDbContext());
                var storageEndpoint = configuration.GetSection("Endpoints").GetValue<string>("Storage");
                var appName = configuration.GetValue<string>("Issuer");

                var userService = GetService<IUserService>();
                var users = userService.GetUsers(); 

                foreach (var user in users)
                {
                    var preference = preferenceRepository.FirstOrDefault(x => x.UserId.Equals(user.Id) && x.Key.Equals("EnableDailyBackups"));

                    if (preference == null) continue;

                    var backupsEnabled = bool.Parse(preference.Value);

                    if (!backupsEnabled) continue;

                    var userdata = userService.DownloadUserData(user.Id);

                    await emailService.Send("no-reply@homemanagement.com",
                        new List<string> { user.Email },
                        "Home Management Back up",
                        "this is a test message",
                        "<strong>this is a test message</strong>");

                    using (var httpClient = new HttpClient())
                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new ByteArrayContent(userdata.GetBuffer()), "file", "userdata.zip");

                        httpClient.DefaultRequestHeaders.Add("Username", user.Email.Substring(0, user.Email.IndexOf("@")));
                        httpClient.DefaultRequestHeaders.Add("AppName", appName);

                        httpClient.BaseAddress = new Uri($"{storageEndpoint}/api/localstorage/send");
                        var response = await httpClient.PutAsync(string.Empty, content);
                        var result = await response.Content.ReadAsStringAsync();
                        response.EnsureSuccessStatusCode();
                    }                    
                }
            }
            catch (Exception ex)
            {
                logger.LogError(1, ex, "Failed to backup users data.");
            }
        }
    }
}
