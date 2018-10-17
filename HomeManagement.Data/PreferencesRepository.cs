using HomeManagement.Domain;

namespace HomeManagement.Data
{
    public class PreferencesRepository : BaseRepository<Preferences>, IPreferencesRepository
    {
        public PreferencesRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(Preferences entity) => GetById(entity.Id) != null;

        public override Preferences GetById(int id) => FirstOrDefault(x => x.Id.Equals(id));
    }
}
