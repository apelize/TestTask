using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Extensions;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly ILogger<UserController> _logger;
    public UserController(IUserRepository repository, ILogger<UserController> logger)
    {
        _logger = logger;
        _repository = repository;
    }
    [HttpGet]
    public async Task<IEnumerable<UserDTO>> GetAllUsers(string sort = null, string order = null, string filter = null, string value = null, int pageIndex = 1, int pageSize = 10)
    {
        return (await _repository.GetAll(filter, value, sort, order, pageIndex, pageSize))
                .Select(user => user.ToUserDTO());
    }

    [Route("id")]
    [HttpGet]
    public async Task<UserDTO> GetUser(int id)
    {
        return (await _repository.Get(id)).ToUserDTO();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _repository.Delete(id);
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(CreateUserDTO newUser)
    {
        await _repository.Add(newUser);
        return Ok(newUser);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateUserDTO user)
    {
        await _repository.Update(user);
        return NoContent();
    }

    [Route("role")]
    [HttpPut]
    public async Task<IActionResult> AddRoleToUser(int userId, int roleId)
    {
        await _repository.AddRole(userId, roleId);
        return NoContent();
    }
}