using HomeManagement.Models;
using System.Collections.Generic;

namespace HomeManagement.Business.Contracts
{
    public interface IAccountService
    {
        OperationResult Add(AccountModel accountModel);

        OperationResult Update(AccountModel accountModel);

        OperationResult Delete(int id);

        IEnumerable<AccountModel> GetAccounts();

        AccountModel Get(int id);

        AccountPageModel Page(AccountPageModel model);
    }
}
