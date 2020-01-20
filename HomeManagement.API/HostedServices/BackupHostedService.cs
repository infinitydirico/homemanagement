using HomeManagement.Business.Contracts;
using HomeManagement.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace HomeManagement.API.HostedServices
{
    public class BackupHostedService : HostedService, IHostedService
    {
        public BackupHostedService(ILogger<HostedService> logger, IServiceScopeFactory factory) 
            : base(logger, factory)
        {
        }

        //public override int GetPeriodToRun() => 60;

        public override int GetPeriodToRun() => 60 * 30;

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

                    using (var httpClient = new HttpClient())
                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new ByteArrayContent(userdata.GetBuffer()), "file", "userdata.zip");

                        httpClient.DefaultRequestHeaders.Add("Username", user.Email.Substring(0, user.Email.IndexOf("@")));
                        httpClient.DefaultRequestHeaders.Add("AppName", appName);

                        httpClient.BaseAddress = new Uri($"{storageEndpoint}/api/storage/send");
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
