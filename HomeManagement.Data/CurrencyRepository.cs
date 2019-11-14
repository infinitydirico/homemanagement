using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HomeManagement.Data
{
    public class CurrencyRepository : BaseRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(DbContext context)
            : base(context)
        {

        }
        public override bool Exists(Currency entity) => GetById(entity.Id) != null;

        public override Currency GetById(int id) =>
            context.Set<Currency>().FirstOrDefault(x => x.Id.Equals(id));
    }
}
