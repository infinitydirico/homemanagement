using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.ProtectedBrowserStorage;
using HomeManagement.Api.Core;
using System.Net.Http;

namespace HomeManagement.API.WebApp.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage protectedSessionStorage;

        public CustomAuthStateProvider(ProtectedSessionStorage protectedSessionStorage)
        {
            this.protectedSessionStorage = protectedSessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await protectedSessionStorage.GetAsync<string>("user");

                if(string.IsNullOrEmpty(token)) return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

                var value = TokenFactory.Reader(token);

                var identity = new ClaimsIdentity(value.Claims, "Fake authentication type");

                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }
    }
}
