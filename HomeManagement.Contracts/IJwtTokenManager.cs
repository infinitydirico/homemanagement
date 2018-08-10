using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace HomeManagement.Contracts
{
    public interface IJwtTokenManager
    {
        string Write(JwtSecurityToken token);

        JwtSecurityToken Read(string token);

        bool CanRead(string token);

        bool HasExpired(SecurityToken token);

        bool IsValid(JwtSecurityToken token);

        bool IsValid(string token);
    }
}
