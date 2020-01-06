using HomeManagement.Core.Extensions;
using HomeManagement.Api.Core;
using HomeManagement.API.Data.Entities;
using HomeManagement.API.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HomeManagement.API.Filters
{
    public class AdminAuthorizationAttribute : AuthorizationAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var header = context.HttpContext.GetAuthorizationHeader();

            if (header.IsEmpty())
            {
                context.Result = new ContentResult { StatusCode = (int)HttpStatusCode.Forbidden, Content = "Header not present" };

                return;
            }

            var token = TokenFactory.Reader(header);

            var email = token.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub));

            var userManager = context.HttpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;

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
