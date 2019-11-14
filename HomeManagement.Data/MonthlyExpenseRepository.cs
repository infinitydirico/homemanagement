using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.Data
{
    public class MonthlyExpenseRepository : BaseRepository<MonthlyExpense>, IMonthlyExpenseRepository
    {
        public MonthlyExpenseRepository(DbContext context)
            : base(context)
        {

        }
        public override bool Exists(MonthlyExpense entity)
            => GetById(entity.Id) != null;

        public override MonthlyExpense GetById(int id)
            => FirstOrDefault(x => x.Id.Equals(id));
    }
}
