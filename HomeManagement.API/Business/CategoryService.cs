using HomeManagement.API.Exportation;
using HomeManagement.Data;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System;
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
        private readonly IUserSessionService userService;
        private readonly IExportableCategory exportableCategory;

        public CategoryService(IAccountRepository accountRepository,
                                    ITransactionRepository transactionRepository,
                                    ICategoryRepository categoryRepository,
                                    ITransactionMapper transactionMapper,
                                    ICategoryMapper categoryMapper,
                                    IUserRepository userRepository,
                                    IUserCategoryRepository userCategoryRepository,
                                    IUserSessionService userService,
                                    IExportableCategory exportableCategory)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.categoryRepository = categoryRepository;
            this.transactionMapper = transactionMapper;
            this.categoryMapper = categoryMapper;
            this.userRepository = userRepository;
            this.userCategoryRepository = userCategoryRepository;
            this.userService = userService;
            this.exportableCategory = exportableCategory;
        }

        public OperationResult Add(CategoryModel categoryModel)
        {
            var entity = categoryMapper.ToEntity(categoryModel);
            var user = userService.GetAuthenticatedUser();

            categoryRepository.Add(entity, user);
            categoryRepository.Commit();

            return OperationResult.Succeed();
        }

        public OperationResult Delete(int id)
        {
            var category = categoryRepository.GetById(id);

            if (category == null) return OperationResult.Succeed();

            var transactionsWithThisCategory = transactionRepository.Count(x => x.CategoryId.Equals(id));

            if (transactionsWithThisCategory > default(int)) return OperationResult.Error($"Category {category.Name} has associated transactions");

            var user = userService.GetAuthenticatedUser();

            categoryRepository.Remove(id, user);
            categoryRepository.Commit();

            return OperationResult.Succeed();
        }

        public FileModel Export()
        {
            var authenticatedUser = userService.GetAuthenticatedUser();

            var categories = categoryRepository.GetUserCategories(authenticatedUser.Email);

            var csv = exportableCategory.ToCsv(categories.ToList());

            var filename = "categories_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";

            return new FileModel
            {
                Name = filename,
                Contents = csv
            };
        }

        public IEnumerable<CategoryModel> GetActive()
        {
            var authenticatedUser = userService.GetAuthenticatedUser();

            var categories = categoryRepository
                .GetActiveUserCategories(authenticatedUser.Email)
                .Select(x => categoryMapper.ToModel(x))
                .ToList();

            return categories;
        }

        public IEnumerable<UserCategoryModel> GetUsersCategories()
        {
            var users = userRepository.GetAll();

            var uc = userCategoryRepository.GetAll();

            var userCategories = from u in users
                                 join c in uc
                                 on u.Id equals c.UserId
                                 let category = categoryRepository.GetById(c.CategoryId)
                                 select new UserCategoryModel
                                 {
                                     User = new UserModel
                                     {
                                         Id = u.Id,
                                         Email = u.Email
                                     },
                                     Category = new CategoryModel
                                     {
                                         Id = c.CategoryId,
                                         IsActive = category.IsActive,
                                         Name = category.Name,
                                         Icon = category.Icon,
                                         IsDefault = category.IsDefault,
                                         Measurable = category.Measurable
                                     }
                                 };

            return userCategories.ToList();
        }

        public void Import(byte[] contents)
        {
            var authenticatedUser = userService.GetAuthenticatedUser();
            var categories = exportableCategory.ToEntities(contents);

            foreach (var entity in categories)
            {
                if (entity == null) continue;
                var category = categoryRepository.FirstOrDefault(x => x.Name.Equals(entity.Name));

                if (category != null && userCategoryRepository.UserHasAssociatedCategory(authenticatedUser.Id, category.Id)) continue;

                entity.Id = 0;

                categoryRepository.Add(entity, authenticatedUser);
            }
            categoryRepository.Commit();
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
        OperationResult Add(CategoryModel categoryModel);

        OperationResult Update(CategoryModel categoryModel);

        OperationResult Delete(int id);

        IEnumerable<CategoryModel> GetActive();

        IEnumerable<UserCategoryModel> GetUsersCategories();

        FileModel Export();

        void Import(byte[] contents);
    }
}
