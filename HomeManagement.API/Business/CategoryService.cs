using HomeManagement.API.Exportation;
using HomeManagement.Contracts.Repositories;
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
        private readonly IUnitOfWork unitOfWork;

        public CategoryService(IAccountRepository accountRepository,
                                    ITransactionRepository transactionRepository,
                                    ICategoryRepository categoryRepository,
                                    ITransactionMapper transactionMapper,
                                    ICategoryMapper categoryMapper,
                                    IUserRepository userRepository,
                                    IUserCategoryRepository userCategoryRepository,
                                    IUserSessionService userService,
                                    IExportableCategory exportableCategory,
                                    IUnitOfWork unitOfWork)
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
            this.unitOfWork = unitOfWork;
        }

        public OperationResult Add(CategoryModel categoryModel)
        {
            var entity = categoryMapper.ToEntity(categoryModel);
            var user = userService.GetAuthenticatedUser();

            categoryRepository.Add(entity, user);
            unitOfWork.Commit();

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
            unitOfWork.Commit();

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
            unitOfWork.Commit();
        }

        public OperationResult Update(CategoryModel categoryModel)
        {
            categoryRepository.Update(categoryMapper.ToEntity(categoryModel));
            unitOfWork.Commit();

            return OperationResult.Succeed();
        }
    }

    public interface ICategoryService
    {
        OperationResult Add(CategoryModel categoryModel);

        OperationResult Update(CategoryModel categoryModel);

        OperationResult Delete(int id);

        IEnumerable<CategoryModel> GetActive();

        FileModel Export();

        void Import(byte[] contents);
    }
}
