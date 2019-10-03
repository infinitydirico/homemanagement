using HomeManagement.Data;
using HomeManagement.Domain;

namespace HomeManagement.API.Business
{
    public class UserSessionService : IUserSessionService
    {
        private readonly IUserRepository userRepository;
        User user;

        public UserSessionService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public User GetAuthenticatedUser() => user;

        public void RegisterScopedUser(string email)
        {
            user = userRepository.GetByEmail(email);
        }
    }

    public interface IUserSessionService
    {
        User GetAuthenticatedUser();

        void RegisterScopedUser(string email);
    }
}
