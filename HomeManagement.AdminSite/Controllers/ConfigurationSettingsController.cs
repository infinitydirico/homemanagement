using HomeManagement.AdminSite.Filters;
using HomeManagement.Business.Contracts;
using HomeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IActionResult Index()
        {
            var configs = GetSettings();
            return View(configs);
        }
        
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var configs = GetSettings();
            var config = configs.First(x => x.Id.Equals(id));
            return View(config);
        }

        [HttpPost]
        public IActionResult Edit(ConfigurationSettingModel model)
        {
            if (ModelState.IsValid)
            {
                configurationSettingsService.Save(model);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
            FlushConsigurationSettingsCache();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(ConfigurationSettingModel model)
        {
            if (ModelState.IsValid)
            {
                configurationSettingsService.Save(model);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
            FlushConsigurationSettingsCache();
            return RedirectToAction("Index");
        }

        private IEnumerable<ConfigurationSettingModel> GetSettings()
        {
            IEnumerable<ConfigurationSettingModel> configs = _cache.Get(nameof(ConfigurationSettingModel)) as IEnumerable<ConfigurationSettingModel>;
            if (configs != null)
            {
                return configs;
            }

            configs = configurationSettingsService.GetConfigs();

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