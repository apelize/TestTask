using DTO;
using Entities;

namespace Extensions;

public static class Extensions
{
    public static UserDTO ToUserDTO(this User user) =>
        new UserDTO(user.Name, user.Age, user.Email, user.Roles.Select(r => r.ToRoleDTO()).ToList<RoleDTO>());

    public static RoleDTO ToRoleDTO(this Role role) =>
        new RoleDTO(role.Access);
}