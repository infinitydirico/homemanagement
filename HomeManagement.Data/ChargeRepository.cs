using HomeManagement.Domain;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace HomeManagement.Data
{
    public class ChargeRepository : BaseRepository<Charge>, IChargeRepository, ITransactonalRepository
    {
        public ChargeRepository(IPlatformContext platformContext) : base(platformContext, new TransactionalRepository<Charge>(platformContext))
        {
        }

        public DbTransaction BeginTransaction()
        {
            return platformContext.BeginTransaction();
        }

        public void CommitData()
        {
            platformContext.GetDbContext().SaveChanges();
        }

        public override bool Exists(Charge entity) => GetById(entity.Id) != null;

        public override Charge GetById(int id) => platformContext.GetDbContext().Set<Charge>().FirstOrDefault(x => x.Id.Equals(id));

        public DbTransaction GetCurrentTransaction()
        {
            throw new System.NotImplementedException();
        }
    }
}
