using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HomeManagement.AdminSite.Controllers
{
    public class TestsController : Controller
    {
        private readonly ILogger<TestsController> logger;
        private readonly IConfiguration configuration;

        public TestsController(IConfiguration configuration, ILogger<TestsController> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Connection(string connection)
        {
            return View(TestsConnection(connection));
        }

        public bool TestsConnection(string connectionString)
        {
            try
            {
                //var connectionString = configuration.GetSection("ConnectionStrings").GetValue<string>("Postgres");
                using (var connection = new Npgsql.NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}