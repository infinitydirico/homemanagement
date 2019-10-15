using HomeManagement.Domain;

namespace HomeManagement.Data
{
    public class MonthlyExpenseRepository : BaseRepository<MonthlyExpense>, IMonthlyExpenseRepository
    {
        public MonthlyExpenseRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(MonthlyExpense entity)
            => GetById(entity.Id) != null;

        public override MonthlyExpense GetById(int id)
            => FirstOrDefault(x => x.Id.Equals(id));
    }
}
