using System.ComponentModel.DataAnnotations;

namespace DTO;

public record CreateUserDTO([Required(ErrorMessage = "Enter user Name")] string Name,
                            [Required(ErrorMessage = "Enter user Age")] int Age,
                            [Required(ErrorMessage = "Enter user Email")] string Email,
                            List<int>? RolesId);