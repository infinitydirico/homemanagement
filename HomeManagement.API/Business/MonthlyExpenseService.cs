using HomeManagement.Data;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.API.Business
{
    public class MonthlyExpenseService : IMonthlyExpenseService
    {
        private readonly IMonthlyExpenseRepository monthlyExpenseRepository;
        private readonly IMonthlyExpenseMapper monthlyExpenseMapper;
        private readonly IUserSessionService userSessionService;

        public MonthlyExpenseService(IMonthlyExpenseRepository monthlyExpenseRepository,
            IMonthlyExpenseMapper monthlyExpenseMapper,
            IUserSessionService userSessionService)
        {
            this.monthlyExpenseRepository = monthlyExpenseRepository;
            this.monthlyExpenseMapper = monthlyExpenseMapper;
            this.userSessionService = userSessionService;
        }

        public IEnumerable<MonthlyExpenseModel> GetMonthlyExpenses()
        {
            var user = userSessionService.GetAuthenticatedUser();

            var MonthlyExpenses = monthlyExpenseRepository
                .Where(x => x.UserId.Equals(user.Id))
                .Select(x => monthlyExpenseMapper.ToModel(x))
                .ToList();

            return MonthlyExpenses;
        }

        public OperationResult Remove(int id)
        {
            var user = userSessionService.GetAuthenticatedUser();

            var entity = monthlyExpenseRepository.GetById(id);

            if (!user.Id.Equals(entity.UserId)) return OperationResult.Error("Not allowed");

            monthlyExpenseRepository.Remove(id);

            monthlyExpenseRepository.Commit();

            return OperationResult.Succeed();
        }

        public OperationResult Save(MonthlyExpenseModel model)
        {
            var user = userSessionService.GetAuthenticatedUser();

            var entity = monthlyExpenseRepository.GetById(model.Id);

            if (entity == null)
            {
                entity = monthlyExpenseMapper.ToEntity(model);
                entity.UserId = user.Id;

                monthlyExpenseRepository.Add(entity);
            }
            else
            {
                if (!user.Id.Equals(entity.Id)) return OperationResult.Error("Not allowed");

                monthlyExpenseRepository.Update(entity);
            }

            monthlyExpenseRepository.Commit();

            return OperationResult.Succeed();
        }
    }

    public interface IMonthlyExpenseService
    {
        OperationResult Save(MonthlyExpenseModel model);

        OperationResult Remove(int id);

        IEnumerable<MonthlyExpenseModel> GetMonthlyExpenses();
    }
}
