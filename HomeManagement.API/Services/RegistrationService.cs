using Grpc.Core;
using HomeManagement.API.Business;
using HomeManagement.API.Protos;
using System.Threading.Tasks;

namespace HomeManagement.API.Services
{
    public class RegistrationService : RegistrationRPC.RegistrationRPCBase
    {
        private readonly IUserService userService;

        public RegistrationService(IUserService userService)
        {
            this.userService = userService;
        }

        public override async Task<Response> NewRegistration(User request, ServerCallContext context)
        {
            userService.CreateDefaultData(new Models.UserModel
            {
                Email = request.Email,
                Language = request.Language
            });

            return await Task.FromResult(new Response());
        }
    }
}
