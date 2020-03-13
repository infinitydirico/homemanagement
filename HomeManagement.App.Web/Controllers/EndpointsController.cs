using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace HomeManagement.App.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EndpointsController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public EndpointsController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var endpoints = from e in configuration.GetSection("Endpoints").GetChildren()
                      select new { e.Key, e.GetSection("url").Value };

            return Ok(endpoints.ToList());
        }
    }
}