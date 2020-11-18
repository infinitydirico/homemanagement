using System;
using System.Net;
using System.Threading.Tasks;
using HomeManagement.Api.Core;
using HomeManagement.Api.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeManagement.Api.Identity.Filters
{
    public class AuthorizationAttribute : Attribute, IAsyncActionFilter
    {
        protected HomeManagementPrincipal principal;

        public AuthorizationAttribute() : this(Constants.Roles.RegularUser) { }

        public AuthorizationAttribute(params string[] policies) => Policies = policies;

        public string[] Policies { get; }

        public virtual async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var header = context.HttpContext.GetAuthorizationHeader();

            if (string.IsNullOrEmpty(header))
            {
                context.Result = new ContentResult { StatusCode = (int)HttpStatusCode.Forbidden, Content = "Header not present" };

                return;
            }

            var token = TokenFactory.Reader(header);

            principal = new HomeManagementPrincipal(token);

            if (principal.Expired())
            {
                context.Result = new ContentResult { StatusCode = (int)HttpStatusCode.Forbidden, Content = "Token has expired" };

                return;
            }

            if (!principal.IsAuthorized(Policies))
            {
                context.Result = new ContentResult { StatusCode = (int)HttpStatusCode.Unauthorized, Content = "Not authorized." };

                return;
            }

            context.HttpContext.User = principal;

            await next();
        }
    }
}