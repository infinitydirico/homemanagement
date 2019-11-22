using HomeManagement.Domain;

namespace HomeManagement.Business.Contracts
{
    public interface IUserSessionService
    {
        User GetAuthenticatedUser();

        void RegisterScopedUser(string email);
    }
}
