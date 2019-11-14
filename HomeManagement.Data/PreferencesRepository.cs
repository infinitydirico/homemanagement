using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.Data
{
    public class PreferencesRepository : BaseRepository<Preferences>, IPreferencesRepository
    {
        public PreferencesRepository(DbContext context)
            : base(context)
        {

        }
        public override bool Exists(Preferences entity) => GetById(entity.Id) != null;

        public override Preferences GetById(int id) => FirstOrDefault(x => x.Id.Equals(id));
    }
}
