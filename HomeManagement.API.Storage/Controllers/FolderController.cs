using HomeManagement.Api.Core;
using HomeManagement.Api.Identity.Filters;
using HomeManagement.API.Storage.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.API.Storage.Controllers
{
    [EnableCors("StorageApiCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorization]
    public class FolderController : ControllerBase
    {
        private readonly IFolderService folderService;

        public FolderController(
            IFolderService folderService)
        {
            this.folderService = folderService;
        }

        private HomeManagementPrincipal Principal => User as HomeManagementPrincipal;

        [HttpGet]
        public IActionResult Get()
        {
            var root = folderService.GetRoot(Principal);
            return Ok(root);
        }

        [HttpGet("{path}")]
        public IActionResult Get([FromQuery] string path)
        {
            if(folderService.IsOutsideUserScope(Principal, path))
            {
                return BadRequest($"Cannot get outside of user scope.");
            }

            var children = folderService.GetDirectory(path);

            return Ok(children);
        }
    }
}
