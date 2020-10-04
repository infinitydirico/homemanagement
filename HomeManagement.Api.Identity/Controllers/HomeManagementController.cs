using HomeManagement.Api.Core;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.Api.Identity.Controllers
{
    public class HomeManagementController : ControllerBase
    {
        public HomeManagementPrincipal Principal => User as HomeManagementPrincipal;
    }
}
