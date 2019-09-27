using HomeManagement.Domain;
using System.Linq;

namespace HomeManagement.Data
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(Transaction entity) => GetById(entity.Id) != null;

        public override Transaction GetById(int id) => dbContext.Set<Transaction>().FirstOrDefault(x => x.Id.Equals(id));

    }
}
