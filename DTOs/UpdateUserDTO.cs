namespace DTO;

#pragma warning disable 1591

public record UpdateUserDTO(int id, string? Name, int? Age, string? Email, List<int>? RolesId);