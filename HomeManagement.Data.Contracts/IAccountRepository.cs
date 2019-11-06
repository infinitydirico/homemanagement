using HomeManagement.Contracts.Repositories;
using HomeManagement.Domain;
using System.Collections.Generic;

namespace HomeManagement.Data
{
    public interface IAccountRepository : IRepository<Account>
    {
        IEnumerable<Account> GetAllByUser(string username);
    }
}
