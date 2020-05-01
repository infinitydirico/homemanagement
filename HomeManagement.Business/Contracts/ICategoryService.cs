using HomeManagement.Models;
using System.Collections.Generic;

namespace HomeManagement.Business.Contracts
{
    public interface ICategoryService
    {
        OperationResult Add(CategoryModel categoryModel);

        OperationResult Update(CategoryModel categoryModel);

        OperationResult Delete(int id);

        IEnumerable<CategoryModel> Get();

        IEnumerable<CategoryModel> GetActive();

        IEnumerable<UserCategoryModel> GetUsersCategories();

        FileModel Export();

        void Import(byte[] contents);
    }
}
