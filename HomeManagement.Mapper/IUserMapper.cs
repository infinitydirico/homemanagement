using HomeManagement.Contracts.Mapper;
using HomeManagement.Domain;
using HomeManagement.Models;

namespace HomeManagement.Mapper
{
    public interface IUserMapper : IMapper<User, UserModel>
    {
    }
}
