using HomeManagement.Data;
using HomeManagement.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace HomeManagement.AdminSite.Controllers
{
    public class PreferencesController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly IRepositoryFactory repositoryFactory;

        public PreferencesController(IRepositoryFactory repositoryFactory,
            IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            this.repositoryFactory = repositoryFactory;
        }

        public IActionResult Index()
        {
            using (var preferencesRepository = repositoryFactory.CreatePreferencesRepository())
            using (var userRepository = repositoryFactory.CreateUserRepository())
            {
                var preferences = preferencesRepository.GetAll();

                var users = userRepository.GetAll();

                foreach (var p in preferences)
                {
                    var user = users.FirstOrDefault(x => x.Id.Equals(p.UserId));

                    if(user != null)
                    {
                        p.User = user;
                    }
                }

                return View(preferences);
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Preferences preference)
        {
            using (var preferencesRepository = repositoryFactory.CreatePreferencesRepository())
            using (var userRepository = repositoryFactory.CreateUserRepository())
            {
                preferencesRepository.Add(preference);
                preferencesRepository.Commit();

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue) return NotFound();
            using (var preferencesRepository = repositoryFactory.CreatePreferencesRepository())
            {
                var preference = preferencesRepository.GetById(id.Value);

                return View(preference);
            }
        }

        [HttpPost]
        public IActionResult Edit(Preferences preferences)
        {
            using (var preferencesRepository = repositoryFactory.CreatePreferencesRepository())
            {
                var entity = preferencesRepository.GetById(preferences.Id);

                entity.Value = preferences.Value;

                preferencesRepository.Update(entity);
                preferencesRepository.Commit();

                return RedirectToAction("Index");
            }
        }
    }
}