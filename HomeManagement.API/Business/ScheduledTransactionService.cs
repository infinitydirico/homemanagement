using HomeManagement.Data;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.API.Business
{
    public class ScheduledTransactionService : IScheduledTransactionService
    {
        private readonly IScheduledTransactionRepository scheduledTransactionRepository;
        private readonly IScheduledTransactionMapper scheduledTransactionMapper;
        private readonly IUserSessionService userSessionService;

        public ScheduledTransactionService(IScheduledTransactionRepository scheduledTransactionRepository,
            IScheduledTransactionMapper scheduledTransactionMapper,
            IUserSessionService userSessionService)
        {
            this.scheduledTransactionRepository = scheduledTransactionRepository;
            this.scheduledTransactionMapper = scheduledTransactionMapper;
            this.userSessionService = userSessionService;
        }

        public IEnumerable<ScheduledTransactionModel> GetScheduledTransactions()
        {
            var user = userSessionService.GetAuthenticatedUser();

            var scheduledTransactions = scheduledTransactionRepository
                .Where(x => x.UserId.Equals(user.Id))
                .Select(x => scheduledTransactionMapper.ToModel(x))
                .ToList();

            return scheduledTransactions;
        }

        public OperationResult Remove(int id)
        {
            var user = userSessionService.GetAuthenticatedUser();

            var entity = scheduledTransactionRepository.GetById(id);

            if (!user.Id.Equals(entity.Id)) return OperationResult.Error("Not allowed");

            scheduledTransactionRepository.Remove(id);

            scheduledTransactionRepository.Commit();

            return OperationResult.Succeed();
        }

        public OperationResult Save(ScheduledTransactionModel model)
        {
            var user = userSessionService.GetAuthenticatedUser();

            var entity = scheduledTransactionRepository.GetById(model.Id);

            if (entity == null)
            {
                entity = scheduledTransactionMapper.ToEntity(model);

                scheduledTransactionRepository.Add(entity);
            }
            else
            {
                if (!user.Id.Equals(entity.Id)) return OperationResult.Error("Not allowed");

                scheduledTransactionRepository.Update(entity);
            }

            scheduledTransactionRepository.Commit();

            return OperationResult.Succeed();
        }
    }

    public interface IScheduledTransactionService
    {
        OperationResult Save(ScheduledTransactionModel model);

        OperationResult Remove(int id);

        IEnumerable<ScheduledTransactionModel> GetScheduledTransactions();
    }
}
