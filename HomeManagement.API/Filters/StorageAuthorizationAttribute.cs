using HomeManagement.Api.Core;
using HomeManagement.API.Business;
using HomeManagement.API.Extensions;
using HomeManagement.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.API.Filters
{
    public class StorageAuthorizationAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var header = context.HttpContext.GetAuthorizationHeader();
            var token = TokenFactory.Reader(header);
            var email = token.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub));

            var userRepository = context.HttpContext.RequestServices.GetService(typeof(IUserRepository)) as IUserRepository;

            var storageService = context.HttpContext.RequestServices.GetService(typeof(IStorageService)) as IStorageService;

            var user = userRepository.FirstOrDefault(x => x.Email.Equals(email.Value));

            if(!storageService.IsAuthorized(user.Id))
            {
                return;
            }

            await next();
        }
    }
}
