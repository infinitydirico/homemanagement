using HomeManagement.Domain;
using System.Linq;

namespace HomeManagement.Data
{
    public class ChargeRepository : BaseRepository<Charge>, IChargeRepository
    {
        public ChargeRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(Charge entity) => GetById(entity.Id) != null;

        public override Charge GetById(int id) => platformContext.GetDbContext().Set<Charge>().FirstOrDefault(x => x.Id.Equals(id));
    }
}
