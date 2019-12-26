using HomeManagement.Business.Contracts;
using HomeManagement.Data;
using HomeManagement.Domain;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace HomeManagement.AdminSite.Services
{
    public class UserSessionService : IUserSessionService
    {
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserSessionService(IRepositoryFactory repositoryFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            this.repositoryFactory = repositoryFactory;
            this.httpContextAccessor = httpContextAccessor;
        }

        public User GetAuthenticatedUser()
        {
            var userRepository = repositoryFactory.CreateUserRepository();
            var email = httpContextAccessor.HttpContext.User.Claims.First(x => x.Type.Equals(JwtRegisteredClaimNames.Sub)).Value;
            var user = userRepository.GetByEmail(email);
            return user;
        }
    }
}
