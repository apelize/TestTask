namespace DTO;

public record CreateUserDTO(string Name, int Age, string Email, List<int> RolesId);