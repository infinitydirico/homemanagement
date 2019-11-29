using HomeManagement.API.Business;
using HomeManagement.API.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HomeManagement.API.HostedServices
{
    public class ListenerHostedService : HostedService
    {
        public ListenerHostedService(ILogger<HostedService> logger, IServiceScopeFactory factory) 
            : base(logger, factory)
        {
        }

        public override int GetPeriodToRun() => 10;

        public override void Process()
        {
            var userService = GetService<IUserService>();
            var listenerService = new ListenerService();

            var result = listenerService.CheckRegistration();

            if (result)
            {
                var usersEmails = listenerService.GetRegisteredUsers();

                foreach (var email in usersEmails)
                {
                    userService.CreateDefaultData(new Models.UserModel
                    {
                        Email = email
                    });
                }                
            }
        }
    }
}