using HomeManagement.Business.Contracts;
using HomeManagement.Data;
using HomeManagement.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace HomeManagement.API.Business
{
    public class UserSessionService : IUserSessionService
    {
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMemoryCache memoryCache;

        public UserSessionService(IRepositoryFactory repositoryFactory,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache)
        {
            this.repositoryFactory = repositoryFactory;
            this.httpContextAccessor = httpContextAccessor;
            this.memoryCache = memoryCache;
        }

        public User GetAuthenticatedUser()
        {
            User user;            

            var email = httpContextAccessor.HttpContext.User.Claims.First(x => x.Type.Equals(JwtRegisteredClaimNames.Sub)).Value;

            var key = $"{nameof(GetAuthenticatedUser)}:{email}";

            user = memoryCache.Get<User>(key);

            if(user == null)
            {
                var userRepository = repositoryFactory.CreateUserRepository();
                user = userRepository.GetByEmail(email);
                memoryCache.CreateEntry(key);
                memoryCache.Set(key, user, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(10)
                });
            }
            
            return user;
        }
    }
}
