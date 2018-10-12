using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace HomeManagement.Data
{
    public class ChargeRepository : BaseRepository<Charge>, IChargeRepository, ITransactonalRepository
    {
        public ChargeRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public IDbContextTransaction BeginTransaction()
        {
            return platformContext.GetDbContext().Database.BeginTransaction();
        }

        public override bool Exists(Charge entity) => GetById(entity.Id) != null;

        public override Charge GetById(int id) => platformContext.GetDbContext().Set<Charge>().FirstOrDefault(x => x.Id.Equals(id));

    }
}
