using HomeManagement.API.Services;
using HomeManagement.Business.Contracts;
using HomeManagement.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace HomeManagement.API.HostedServices
{
    public class EmailHostedService : HostedService, IHostedService
    {
        private readonly IEmailService emailService;

        public EmailHostedService(ILogger<HostedService> logger,
            IServiceScopeFactory factory,
            IEmailService emailService) 
            : base(logger, factory)
        {
            this.emailService = emailService;
        }

        public override int GetPeriodToRun() => 60 * 60 * 24 * 7;

        public async override void Process()
        {
            var userService = GetService<IUserService>();
            var users = userService.GetUsers();

            var context = GetService<IPlatformContext>();
            var preferenceRepository = new PreferencesRepository(context.CreateDbContext());

            foreach (var user in users)
            {
                //will have to handle this with a flag
                if (user.Email.Contains("test")) continue;

                var preference = preferenceRepository.FirstOrDefault(x => x.UserId.Equals(user.Id) && x.Key.Equals("EnableWeeklyEmails"));
                if (preference == null) continue;

                var weeklyEmails = bool.Parse(preference.Value);

                if (!weeklyEmails) continue;

                var userdata = userService.DownloadUserData(user.Id);

                var bytes = userdata.GetBuffer();

                await emailService.Send("no-reply@homemanagement.com",
                        new List<string> { user.Email },
                        "Home Management Weekly back up",
                        "Here's your weeklt backup",
                        "<strong>Here's your weeklt backup</strong>",
                        "userdata.zip",
                        Convert.ToBase64String(bytes));
            }
        }
    }
}
