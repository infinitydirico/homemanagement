using HomeManagement.Contracts.Mapper;
using HomeManagement.Domain;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Reflection;

namespace HomeManagement.Mapper
{
    public class ScheduledTransactionMapper : BaseMapper<ScheduledTransaction, ScheduledTransactionModel>, IScheduledTransactionMapper
    {
        public override IEnumerable<PropertyInfo> GetEntityProperties()
        {
            yield return typeof(ScheduledTransaction).GetProperty(nameof(ScheduledTransaction.Id));
            yield return typeof(ScheduledTransaction).GetProperty(nameof(ScheduledTransaction.Name));
            yield return typeof(ScheduledTransaction).GetProperty(nameof(ScheduledTransaction.Price));
            yield return typeof(ScheduledTransaction).GetProperty(nameof(ScheduledTransaction.TransactionType));
            yield return typeof(ScheduledTransaction).GetProperty(nameof(ScheduledTransaction.CategoryId));
        }

        public override IEnumerable<PropertyInfo> GetModelProperties()
        {
            yield return typeof(ScheduledTransactionModel).GetProperty(nameof(ScheduledTransactionModel.Id));
            yield return typeof(ScheduledTransactionModel).GetProperty(nameof(ScheduledTransactionModel.Name));
            yield return typeof(ScheduledTransactionModel).GetProperty(nameof(ScheduledTransactionModel.Price));
            yield return typeof(ScheduledTransactionModel).GetProperty(nameof(ScheduledTransactionModel.TransactionType));
            yield return typeof(ScheduledTransactionModel).GetProperty(nameof(ScheduledTransactionModel.CategoryId));
        }
    }

    public interface IScheduledTransactionMapper : IMapper<ScheduledTransaction, ScheduledTransactionModel>
    {

    }
}
