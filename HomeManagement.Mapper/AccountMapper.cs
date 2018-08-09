using HomeManagement.Contracts.Mapper;
using HomeManagement.Domain;
using HomeManagement.Models;

namespace HomeManagement.Mapper
{
    public class AccountMapper : BaseMapper<Account, AccountModel>, IAccountMapper
    {
    }
}
