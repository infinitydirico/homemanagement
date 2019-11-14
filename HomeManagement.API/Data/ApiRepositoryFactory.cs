using HomeManagement.API.Data.Repositories;
using HomeManagement.Data;

namespace HomeManagement.API.Data
{
    public class ApiRepositoryFactory : RepositoryFactory, IApiRepositoryFactory
    {
        public ApiRepositoryFactory(IPlatformContext platformContext) 
            : base(platformContext)
        {
        }

        public ITokenRepository CreateTokenRepository() => new TokenRepository(context);
    }

    public interface IApiRepositoryFactory
    {
        ITokenRepository CreateTokenRepository();
    }
}
