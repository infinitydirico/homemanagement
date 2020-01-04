using HomeManagement.Models;
using System.Collections.Generic;

namespace HomeManagement.Business.Contracts
{
    public interface IMonthlyExpenseService
    {
        OperationResult Save(MonthlyExpenseModel model);

        OperationResult Remove(int id);

        IEnumerable<MonthlyExpenseModel> GetMonthlyExpenses();
    }
}
