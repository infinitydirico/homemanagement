using HomeManagement.Api.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.Filters
{
    public class AdminAuthorizationAttribute : AuthorizationAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var header = context.HttpContext?.Request?.Headers?["Authorization"].FirstOrDefault();

            var token = TokenFactory.Reader(header);

            var email = token.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub));

            var userManager = context.HttpContext.RequestServices.GetService(typeof(UserManager<IdentityUser>)) as UserManager<IdentityUser>;

            var user = await userManager.FindByEmailAsync(email.Value);

            var userRoles = await userManager.GetRolesAsync(user);

            if (!userRoles.Contains("Administrator"))
            {
                context.Result = new ContentResult { StatusCode = (int)HttpStatusCode.Unauthorized };
                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
