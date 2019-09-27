using System.Collections.Generic;
using System.Reflection;
using HomeManagement.Domain;
using HomeManagement.Models;

namespace HomeManagement.Mapper
{
    public class TransactionMapper : BaseMapper<Transaction, TransactionModel>, ITransactionMapper
    {
        public override Transaction ToEntity(TransactionModel model)
        {
            return new Transaction
            {
                Id = model.Id,
                AccountId = model.AccountId,
                CategoryId = model.CategoryId,
                TransactionType = model.TransactionType == TransactionTypeModel.Income ? TransactionType.Income : TransactionType.Expense,
                Date = model.Date,
                Name = model.Name,
                Price = model.Price
            };
        }

        public override TransactionModel ToModel(Transaction entity)
        {
            return new TransactionModel
            {
                Id = entity.Id,
                AccountId = entity.AccountId,
                CategoryId = entity.CategoryId,
                TransactionType = entity.TransactionType == TransactionType.Income ? TransactionTypeModel.Income : TransactionTypeModel.Expense,
                Date = entity.Date,
                Name = entity.Name,
                Price = entity.Price
            };
        }

        public override IEnumerable<PropertyInfo> GetEntityProperties()
        {
            yield return typeof(Transaction).GetProperty(nameof(Transaction.Id));
            yield return typeof(Transaction).GetProperty(nameof(Transaction.Name));
            yield return typeof(Transaction).GetProperty(nameof(Transaction.Price));
            yield return typeof(Transaction).GetProperty(nameof(Transaction.Date));
            yield return typeof(Transaction).GetProperty(nameof(Transaction.TransactionType));
            yield return typeof(Transaction).GetProperty(nameof(Transaction.AccountId));
            yield return typeof(Transaction).GetProperty(nameof(Transaction.CategoryId));
        }

        public override IEnumerable<PropertyInfo> GetModelProperties()
        {
            yield return typeof(TransactionModel).GetProperty(nameof(TransactionModel.Id));
            yield return typeof(TransactionModel).GetProperty(nameof(TransactionModel.Name));
            yield return typeof(TransactionModel).GetProperty(nameof(TransactionModel.Price));
            yield return typeof(TransactionModel).GetProperty(nameof(TransactionModel.Date));
            yield return typeof(TransactionModel).GetProperty(nameof(TransactionModel.TransactionType));
            yield return typeof(TransactionModel).GetProperty(nameof(TransactionModel.AccountId));
            yield return typeof(TransactionModel).GetProperty(nameof(TransactionModel.CategoryId));
        }
    }
}
