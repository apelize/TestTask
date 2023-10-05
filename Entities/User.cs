namespace Entities;

public class User
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required int Age { get; set; }
    public required string Email { get; set; }
    public required List<Role> Roles { get; set; }
}