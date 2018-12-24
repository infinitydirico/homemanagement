using HomeManagement.Domain;
using System.Linq;

namespace HomeManagement.Data
{
    public class CurrencyRepository : BaseRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(Currency entity) => GetById(entity.Id) != null;

        public override Currency GetById(int id) =>
            platformContext.GetDbContext().Set<Currency>().FirstOrDefault(x => x.Id.Equals(id));
    }
}
