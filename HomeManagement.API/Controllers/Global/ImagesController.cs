using HomeManagement.API.Business;
using HomeManagement.Core.Extensions;
using HomeManagement.API.Filters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HomeManagement.API.Controllers.Global
{
    [Authorization]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/Images")]
    public class ImagesController : Controller
    {
        private readonly IImageService imageService;

        public ImagesController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        [HttpGet("isconfigured")]
        public IActionResult IsConfigured()
        {
            return Ok(imageService.IsConfigured());
        }

        [HttpPost]
        public async Task<IActionResult> GetText()
        {
            if (!imageService.IsConfigured()) return Forbid("Image api is disabled.");

            var transaction = await imageService.CreateTransactionFromImage(Request.Body.GetBytes());
            return Ok(transaction);
        }
    }
}