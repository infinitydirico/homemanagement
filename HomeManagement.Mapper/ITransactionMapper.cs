using HomeManagement.Contracts.Mapper;
using HomeManagement.Domain;
using HomeManagement.Models;

namespace HomeManagement.Mapper
{
    public interface ITransactionMapper : IMapper<Transaction, TransactionModel>
    {
    }
}
