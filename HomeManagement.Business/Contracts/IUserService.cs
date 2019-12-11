using HomeManagement.Models;
using System.Collections.Generic;
using System.IO;

namespace HomeManagement.Business.Contracts
{
    public interface IUserService
    {
        OperationResult CreateUser(string email, string language);

        MemoryStream DownloadUserData(int userId);

        OperationResult DeleteUser(int userId);

        IEnumerable<UserModel> GetUsers();
    }
}
