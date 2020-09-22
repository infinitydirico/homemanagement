using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace HomeManagement.API.Infraestructure
{
    public class HomeManagementPrincipal : ClaimsPrincipal
    {
        public HomeManagementPrincipal(IEnumerable<Claim> claims)
            :base(new ClaimsIdentity(claims))
        {
            Email = claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub)).Value;
        }

        public string Email { get; }
    }

    public static class HomeManagementPrincipalExtensions
    {
        public static string GetUserEmail(this ClaimsPrincipal principal) => (principal as HomeManagementPrincipal).Email;
    }
}
