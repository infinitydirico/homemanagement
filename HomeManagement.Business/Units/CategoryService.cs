using HomeManagement.Business.Contracts;
using HomeManagement.Data;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.Business.Units
{
    public class CategoryService : ICategoryService
    {
        private readonly ITransactionMapper transactionMapper;
        private readonly ICategoryMapper categoryMapper;
        private readonly IUserSessionService userService;
        private readonly IExportableCategory exportableCategory;
        private readonly IRepositoryFactory repositoryFactory;

        public CategoryService(ITransactionMapper transactionMapper,
                                    ICategoryMapper categoryMapper,
                                    IUserSessionService userService,
                                    IExportableCategory exportableCategory,
                                    IRepositoryFactory repositoryFactory)
        {
            this.transactionMapper = transactionMapper;
            this.categoryMapper = categoryMapper;
            this.userService = userService;
            this.exportableCategory = exportableCategory;
            this.repositoryFactory = repositoryFactory;
        }

        public OperationResult Add(CategoryModel categoryModel)
        {
            using (var categoryRepository = repositoryFactory.CreateCategoryRepository())
            {
                var entity = categoryMapper.ToEntity(categoryModel);
                var user = userService.GetAuthenticatedUser();
                entity.UserId = user.Id;

                categoryRepository.Add(entity);
                categoryRepository.Commit();

                return OperationResult.Succeed();
            }
        }

        public OperationResult Delete(int id)
        {
            using (var categoryRepository = repositoryFactory.CreateCategoryRepository())
            using (var transactionRepository = repositoryFactory.CreateTransactionRepository())
            {
                var category = categoryRepository.GetById(id);

                if (category == null) return OperationResult.Succeed();

                var transactionsWithThisCategory = transactionRepository.Count(x => x.CategoryId.Equals(id));

                if (transactionsWithThisCategory > default(int)) return OperationResult.Error($"Category {category.Name} has associated transactions");

                var user = userService.GetAuthenticatedUser();

                if (user.Id.Equals(category.UserId))
                {
                    categoryRepository.Remove(id);
                    categoryRepository.Commit();
                }
                else
                {
                    throw new ApplicationException("Cannot remove a category from another user.");
                }

                return OperationResult.Succeed();
            }
        }

        public FileModel Export()
        {
            var categoryRepository = repositoryFactory.CreateCategoryRepository();

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

        public IEnumerable<CategoryModel> Get()
        {
            var authenticatedUser = userService.GetAuthenticatedUser();

            var categoryRepository = repositoryFactory.CreateCategoryRepository();

            var categories = categoryRepository.GetUserCategories(authenticatedUser.Email);

            return categoryMapper.ToModels(categories);
        }

        public IEnumerable<CategoryModel> GetActive()
        {
            var categoryRepository = repositoryFactory.CreateCategoryRepository();

            var authenticatedUser = userService.GetAuthenticatedUser();

            var categories = categoryRepository
                .GetActiveUserCategories(authenticatedUser.Email)
                .Select(x => categoryMapper.ToModel(x))
                .ToList();

            return categories;
        }

        public IEnumerable<UserCategoryModel> GetUsersCategories()
        {
            using (var categoryRepository = repositoryFactory.CreateCategoryRepository())
            using (var userRepository = repositoryFactory.CreateUserRepository())
            {
                var users = userRepository.GetAll();

                return categoryRepository
                    .GetAll()
                    .Select(category =>
                    {
                        var u = users.First(x => x.Id.Equals(category.UserId));
                        return new UserCategoryModel
                        {
                            User = new UserModel
                            {
                                Id = u.Id,
                                Email = u.Email
                            },
                            Category = new CategoryModel
                            {
                                Id = category.Id,
                                IsActive = category.IsActive,
                                Name = category.Name,
                                Icon = category.Icon,
                                Measurable = category.Measurable,
                                UserId = u.Id
                            }
                        };
                    })
                    .ToList();
            }
        }

        public void Import(byte[] contents)
        {
            using (var categoryRepository = repositoryFactory.CreateCategoryRepository())
            {
                var authenticatedUser = userService.GetAuthenticatedUser();
                var categories = exportableCategory.ToEntities(contents);

                foreach (var entity in categories)
                {
                    if (entity == null) continue;
                    var category = categoryRepository.FirstOrDefault(x => x.Name.Equals(entity.Name));

                    entity.Id = 0;
                    entity.UserId = authenticatedUser.Id;

                    categoryRepository.Add(entity);
                }
                categoryRepository.Commit();
            }
        }

        public OperationResult Update(CategoryModel categoryModel)
        {
            using (var categoryRepository = repositoryFactory.CreateCategoryRepository())
            {
                categoryRepository.Update(categoryMapper.ToEntity(categoryModel));
                categoryRepository.Commit();

                return OperationResult.Succeed();
            }
        }
    }
}
