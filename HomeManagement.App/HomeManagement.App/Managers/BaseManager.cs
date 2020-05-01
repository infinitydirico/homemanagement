using HomeManagement.App.Data;
using HomeManagement.App.Data.Entities;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.App.Managers
{
    public class BaseManager<TEntity, TPage>
        where TEntity : class, IOfflineEntity
        where TPage : Page, new()
    {
        protected readonly GenericRepository<TEntity> repository = new GenericRepository<TEntity>();
        protected readonly GenericRepository<AppSettings> appSettingsRepository = new GenericRepository<AppSettings>();

        protected TPage page = new TPage();
        protected AppSettings coudSyncSetting;

        public BaseManager()
        {
            coudSyncSetting = appSettingsRepository.FirstOrDefault(x => x.Name.Equals(AppSettings.GetOfflineModeSetting().Name));
        }

        public virtual async Task<IEnumerable<TEntity>> NextPageAsync()
        {
            page.CurrentPage++;

            return await Paginate();
        }


        public virtual async Task<IEnumerable<TEntity>> PreviousPageAsync()
        {
            page.CurrentPage--;

            return await Paginate();
        }

        protected virtual async Task<IEnumerable<TEntity>> Paginate()
        {
            return await Task.FromResult(Enumerable.Empty<TEntity>());
        }
    }
}
