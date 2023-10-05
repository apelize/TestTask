using Entities;

namespace DTO;

public record UserDTO(string Name, int Age, string Email, List<RoleDTO> Roles);