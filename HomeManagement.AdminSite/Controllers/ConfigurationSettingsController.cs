using HomeManagement.AdminSite.Filters;
using HomeManagement.AdminSite.Services;
using HomeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.AdminSite.Controllers
{
    public class ConfigurationSettingsController : Controller
    {
        private readonly IConfigurationSettingsService configurationSettingsService;

        public ConfigurationSettingsController(IConfigurationSettingsService configurationSettingsService)
        {
            this.configurationSettingsService = configurationSettingsService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var configs = await configurationSettingsService.GetSettings();
            return View(configs);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var configs = await configurationSettingsService.GetSettings();
            var config = configs.First(x => x.Id.Equals(id));
            return View(config);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ConfigurationSettingModel model)
        {
            if (ModelState.IsValid)
            {
                await configurationSettingsService.Update(model);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index");
        }
    }
}