using Entities;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable 1591

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly ApplicationDBContext _context;
    public RoleController(ApplicationDBContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    [HttpGet]
    public Task<IEnumerable<Role>> GetRoles()
    {
        return Task.FromResult(_context.Roles.Select(r => r).AsEnumerable());
    }
}