using HomeManagement.Api.Core;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Controllers
{
    public class HomeManagementController : ControllerBase
    {
        public HomeManagementPrincipal Principal => User as HomeManagementPrincipal;
    }
}
