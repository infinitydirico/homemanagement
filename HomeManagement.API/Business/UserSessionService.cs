using HomeManagement.Business.Contracts;
using HomeManagement.Data;
using HomeManagement.Domain;

namespace HomeManagement.API.Business
{
    public class UserSessionService : IUserSessionService
    {
        private readonly IRepositoryFactory repositoryFactory;
        User user;

        public UserSessionService(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public User GetAuthenticatedUser() => user;

        public void RegisterScopedUser(string email)
        {
            var userRepository = repositoryFactory.CreateUserRepository();
            user = userRepository.GetByEmail(email);
        }
    }
}
