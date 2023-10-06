using DTO;
using Entities;

#pragma warning disable 1591

namespace Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task AddUser(CreateUserDTO entity);
    Task AddRole(int id, int roleId);
    Task Update(UpdateUserDTO entity);

}