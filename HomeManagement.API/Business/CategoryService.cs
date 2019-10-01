using HomeManagement.Data;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.API.Business
{
    public class CategoryService : ICategoryService
    {
        private readonly IAccountRepository accountRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ITransactionMapper transactionMapper;
        private readonly ICategoryMapper categoryMapper;
        private readonly IUserCategoryRepository userCategoryRepository;

        public CategoryService(IAccountRepository accountRepository,
                                    ITransactionRepository transactionRepository,
                                    ICategoryRepository categoryRepository,
                                    ITransactionMapper transactionMapper,
                                    ICategoryMapper categoryMapper,
                                    IUserRepository userRepository,
                                    IUserCategoryRepository userCategoryRepository)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.categoryRepository = categoryRepository;
            this.transactionMapper = transactionMapper;
            this.categoryMapper = categoryMapper;
            this.userRepository = userRepository;
            this.userCategoryRepository = userCategoryRepository;
        }

        public OperationResult Add(CategoryModel categoryModel, string userEmail)
        {
            var entity = categoryMapper.ToEntity(categoryModel);
            var user = userRepository.FirstOrDefault(x => x.Email.Equals(userEmail));

            categoryRepository.Add(entity, user);
            categoryRepository.Commit();

            return OperationResult.Succeed();
        }

        public OperationResult Delete(int id, string userEmail)
        {
            var category = categoryRepository.GetById(id);

            if (category == null) return OperationResult.Succeed();

            var transactionsWithThisCategory = transactionRepository.Count(x => x.CategoryId.Equals(id));

            if (transactionsWithThisCategory > default(int)) return OperationResult.Error($"Category {category.Name} has associated transactions");

            var user = userRepository.FirstOrDefault(x => x.Email.Equals(userEmail));

            categoryRepository.Remove(id, user);
            categoryRepository.Commit();

            return OperationResult.Succeed();
        }

        public IEnumerable<CategoryModel> GetActive(string userEmail)
        {
            var categories = (from category in categoryRepository.All
                              join userCategory in userCategoryRepository.All
                              on category.Id equals userCategory.CategoryId
                              join user in userRepository.All
                              on userCategory.UserId equals user.Id
                              where user.Email.Equals(userEmail) && category.IsActive
                              select categoryMapper.ToModel(category)).ToList();

            return categories;
        }

        public OperationResult Update(CategoryModel categoryModel)
        {
            categoryRepository.Update(categoryMapper.ToEntity(categoryModel));
            categoryRepository.Commit();

            return OperationResult.Succeed();
        }
    }

    public interface ICategoryService
    {
        OperationResult Add(CategoryModel categoryModel, string userEmail);

        OperationResult Update(CategoryModel categoryModel);

        OperationResult Delete(int id, string userEmail);

        IEnumerable<CategoryModel> GetActive(string userEmail);
    }
}
