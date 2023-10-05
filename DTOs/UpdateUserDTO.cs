namespace DTO;

public record UpdateUserDTO(int id, string? Name, int? Age, string? Email, List<int>? RolesId);