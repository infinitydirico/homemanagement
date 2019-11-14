using HomeManagement.API.Filters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HomeManagement.API.Controllers.Administration
{
    [Authorization]
    [AdminAuthorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Administration")]
    public class AdministrationController : Controller
    {
        [HttpGet("GetMemory")]
        public IActionResult GetMemory()
        {
            var process = System.Diagnostics.Process.GetCurrentProcess();
            var info = new
            {
                GcMemory = GC.GetTotalMemory(true),
                VirtualMemory = process.VirtualMemorySize64,
                Memory = process.PrivateMemorySize64
            };

            return Ok(info);
        }

        [HttpPost("ReleaseMemory")]
        public IActionResult ReleaseMemory(int gen)
        {
            GC.Collect(gen, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();

            return Ok();
        }
    }
}