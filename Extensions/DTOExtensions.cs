using DTO;
using Entities;

namespace Extensions;

public static partial class Extensions
{
    public static UserDTO ToUserDTO(this User user) =>
        new UserDTO(
            user.Name,
            user.Age,
            user.Email,
            String.Join(',', user.Status.Select(roleId => 
                roleId.ToRoleDTO())
                .ToList<RoleDTO>()
                .Select(role => role.Access)));

    public static RoleDTO ToRoleDTO(this Role role) =>
        new RoleDTO(role.Access);
}