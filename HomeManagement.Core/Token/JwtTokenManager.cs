//using HomeManagement.Contracts;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.IdentityModel.Tokens.Jwt;

//namespace HomeManagement.Core.Token
//{
//    public class JwtTokenManager : IJwtTokenManager
//    {
//        private readonly JwtSecurityTokenHandler jwtSecurityToken = new JwtSecurityTokenHandler();

//        public bool CanRead(string token) => jwtSecurityToken.CanReadToken(token);

//        public bool HasExpired(SecurityToken token) => (token.ValidTo - DateTime.UtcNow).TotalDays < default(int);

//        public bool IsValid(JwtSecurityToken token) => (DateTime.UtcNow - token.ValidFrom).TotalHours < 1;

//        public bool IsValid(string token)
//        {
//            if (!CanRead(token)) return false;

//            var jwtToken = Read(token);

//            return HasExpired(jwtToken) || IsValid(jwtToken);
//        }

//        public JwtSecurityToken Read(string token) => jwtSecurityToken.ReadJwtToken(token);

//        public string Write(JwtSecurityToken token) => jwtSecurityToken.WriteToken(token);
//    }
//}
