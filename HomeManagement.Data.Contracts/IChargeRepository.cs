using HomeManagement.Contracts.Repositories;
using HomeManagement.Domain;

namespace HomeManagement.Data
{
    public interface IChargeRepository : IRepository<Charge>, ITransactonalRepository
    {
    }
}
