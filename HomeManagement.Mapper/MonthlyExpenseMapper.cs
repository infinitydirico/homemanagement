using HomeManagement.Contracts.Mapper;
using HomeManagement.Domain;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Reflection;

namespace HomeManagement.Mapper
{
    public class MonthlyExpenseMapper : BaseMapper<MonthlyExpense, MonthlyExpenseModel>, IMonthlyExpenseMapper
    {
        public override IEnumerable<PropertyInfo> GetEntityProperties()
        {
            yield return typeof(MonthlyExpense).GetProperty(nameof(MonthlyExpense.Id));
            yield return typeof(MonthlyExpense).GetProperty(nameof(MonthlyExpense.Name));
            yield return typeof(MonthlyExpense).GetProperty(nameof(MonthlyExpense.Price));
            yield return typeof(MonthlyExpense).GetProperty(nameof(MonthlyExpense.TransactionType));
            yield return typeof(MonthlyExpense).GetProperty(nameof(MonthlyExpense.CategoryId));
        }

        public override IEnumerable<PropertyInfo> GetModelProperties()
        {
            yield return typeof(MonthlyExpenseModel).GetProperty(nameof(MonthlyExpenseModel.Id));
            yield return typeof(MonthlyExpenseModel).GetProperty(nameof(MonthlyExpenseModel.Name));
            yield return typeof(MonthlyExpenseModel).GetProperty(nameof(MonthlyExpenseModel.Price));
            yield return typeof(MonthlyExpenseModel).GetProperty(nameof(MonthlyExpenseModel.TransactionType));
            yield return typeof(MonthlyExpenseModel).GetProperty(nameof(MonthlyExpenseModel.CategoryId));
        }

        public override MonthlyExpense ToEntity(MonthlyExpenseModel model)
        {
            return new MonthlyExpense
            {
                Id = model.Id,
                Name = model.Name,
                CategoryId = model.CategoryId,
                Price = model.Price,
                TransactionType = model.TransactionType.Equals(TransactionTypeModel.Expense) ?
                                        TransactionType.Expense :
                                        TransactionType.Income
            };
        }

        public override MonthlyExpenseModel ToModel(MonthlyExpense entity)
        {
            return new MonthlyExpenseModel
            {
                Id = entity.Id,
                Name = entity.Name,
                CategoryId = entity.CategoryId,
                Price = entity.Price,
                TransactionType = entity.TransactionType.Equals(TransactionType.Expense) ?
                                        TransactionTypeModel.Expense :
                                        TransactionTypeModel.Income
            };
        }
    }

    public interface IMonthlyExpenseMapper : IMapper<MonthlyExpense, MonthlyExpenseModel>
    {

    }
}
