using HomeManagement.AdminSite.Filters;
using HomeManagement.AdminSite.Services;
using HomeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.AdminSite.Controllers
{
    [Authorize]
    public class ConfigurationSettingsController : Controller
    {
        private readonly IConfigurationSettingsService configurationSettingsService;
        private readonly IMemoryCache _cache;

        public ConfigurationSettingsController(IConfigurationSettingsService configurationSettingsService,
            IMemoryCache cache)
        {
            this.configurationSettingsService = configurationSettingsService;
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            var configs = await GetSettings();
            return View(configs);
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var configs = await GetSettings();
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
            FlushConsigurationSettingsCache();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ConfigurationSettingModel model)
        {
            if (ModelState.IsValid)
            {
                await configurationSettingsService.Update(model);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
            FlushConsigurationSettingsCache();
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<ConfigurationSettingModel>> GetSettings()
        {
            IEnumerable<ConfigurationSettingModel> configs = _cache.Get(nameof(ConfigurationSettingModel)) as IEnumerable<ConfigurationSettingModel>;
            if (configs != null)
            {
                return configs;
            }

            configs = await configurationSettingsService.GetSettings();

            _cache.CreateEntry(nameof(ConfigurationSettingModel));
            _cache.Set(nameof(ConfigurationSettingModel), configs, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(10)
            });

            return configs;
        }

        private void FlushConsigurationSettingsCache()
        {
            _cache.Remove(nameof(ConfigurationSettingModel));
        }
    }
}