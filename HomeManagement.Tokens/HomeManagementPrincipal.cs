using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace HomeManagement.Api.Core
{
    public class HomeManagementPrincipal : ClaimsPrincipal
    {
        public HomeManagementPrincipal(JwtSecurityToken token)
            : base(new ClaimsIdentity(token.Claims))
        {
            Id = token.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub)).Value;
            Email = token.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            ExpirationDate = token.ValidTo;
            Roles = token.Claims.Where(x => x.Type.Equals(ClaimTypes.Role)).Select(x => x.Value).ToArray();
            ApplicationName = token.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Iss)).Value;
            Audience = token.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Aud)).Value;
            Name = token.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value;
        }

        public string Id { get; }

        public string Email { get; }

        public string Name { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string[] Roles { get; }

        public string ApplicationName { get; }

        public string Audience { get; }

        public bool Expired() => (ExpirationDate - DateTime.UtcNow).TotalDays < default(int);

        public bool IsAuthorized(string[] policies)
        {
            return (from p in policies
                    join r in Roles
                    on p equals r
                    select r).Any();
        }
    }
}
