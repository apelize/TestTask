using DTO;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Extensions;

[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    ApplicationDBContext _context;
    public TestController(ApplicationDBContext context)
    {
        _context = context;
    }
    [HttpGet]
    public Task<List<UserDTO>> GetAllUsers()
    {
        IEnumerable<User> users = (_context.Users.Include(u => u.Roles).ToList<User>());
        return Task.FromResult(users.Select(u => u.ToUserDTO()).ToList<UserDTO>());
    }
}