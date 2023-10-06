using DTO;
using Microsoft.AspNetCore.Mvc;
using Extensions;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserRepository repository, ILogger<UserController> logger)
    {
        _logger = logger;
        _repository = repository;
    }

    /// <summary>
    ///Get All Users
    ///</summary>
    /// <param name='sort' example='Name, Age, Email'>Parameter for sorting users</param>
    /// <param name='order' example='ASC, DESC'>Sorting order</param>
    /// <param name='filter' example='Name, Age, Email, Roles'>Parameter to filter users</param>
    /// <param name='value' example='John, 20, Admin'>Filter parameter value</param>
    /// <param name='pageIndex'>Page index</param>
    /// <param name='pageSize'>Page size</param>
    [HttpGet]
    public async Task<IEnumerable<UserDTO>> GetAllUsers(string sort = null!, string order = null!, string filter = null!, string value = null!, int pageIndex = 1, int pageSize = 10)
    {
        _logger.LogInformation($"GET all request");

        return (await _repository.GetAll(filter, value, sort, order, pageIndex, pageSize))
            .Select(user => user.ToUserDTO());
    }

    /// <summary>
    /// Get User by Id
    /// </summary>
    [Route("{id}")]
    [HttpGet]
    public async Task<ActionResult<UserDTO>> GetUser(int id)
    {
        _logger.LogInformation($"GET request by id {0}", id);

        try
        {
            return (await _repository.Get(id)).ToUserDTO();
        }
        catch (ArgumentNullException)
        {
            return NotFound($"User with id {id} wasn't found");
        }
    }

    /// <summary>
    ///Delete User by Id
    ///</summary>
    [Route("{id}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(int id)
    {
        _logger.LogInformation($"DELETE request by id {0}", id);

        await _repository.Delete(id);
        return NoContent();
    }

    /// <summary>
    /// Add user
    /// </summary>
    /// <remarks>
    /// Request Example
    ///
    ///     POST /users
    ///     {
    ///         "name" : "John",
    ///         "age" : 20,
    ///         "email": "john@mail.com",
    ///         "rolesId": [
    ///             2
    ///          ]
    ///     }
    ///
    /// </remarks>
    /// <param name='newUser'></param>
    [HttpPost]
    public async Task<IActionResult> AddUser(CreateUserDTO newUser)
    {
        _logger.LogInformation("POST user request Name: {0}, Age: {1}, Email: {2}, Roles: {3}", newUser.Name, newUser.Age, newUser.Email, newUser.RolesId is null ? "" : String.Join(",", newUser.RolesId));

        try
        {
            await EnsureDistinctEmail(newUser);
            EnsurePositiveAge(newUser);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }

        await _repository.AddUser(newUser);
        return Ok(newUser);
    }

    /// <summary>
    /// Update user by Id
    /// </summary>
    /// <remarks>
    /// Request Example
    ///
    ///     PUT /users
    ///     {
    ///       "id" : 1
    ///       "name" : "Jane",
    ///       "age" : 25,
    ///       "email": "jane@mail.com",
    ///       "rolesId": [
    ///           2,
    ///           3
    ///        ]
    ///     }
    ///
    /// </remarks>
    /// <param name='user'></param>
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateUserDTO user)
    {
        _logger.LogInformation("PUT user request Id: {0} Name: {1}, Age: {2}, Email: {3}, Roles: {4}", user.id, user.Name, user.Age, user.Email, user.RolesId is null ? "" : String.Join(",", user.RolesId));

        try
        {
            await _repository.Update(user);
            return NoContent();
        }
        catch (ArgumentNullException)
        {
            return NotFound($"User with id {user.id} wasn't found");
        }
    }

    /// <summary>
    /// Add role to user by Id
    /// </summary>
    [Route("role")]
    [HttpPut]
    public async Task<IActionResult> AddRoleToUser(int userId, int roleId)
    {
        _logger.LogInformation("PUT role request userId: {0}, roleId: {1}", userId, roleId);

        try
        {
            await _repository.AddRole(userId, roleId);
            return NoContent();
        }
        catch (ArgumentNullException)
        {
            return NotFound($"User with id {userId} wasn't found");
        }
    }

    private async Task EnsureDistinctEmail(CreateUserDTO newUser)
    {
        if ((await _repository.GetAll()).Select(user => user.Email).Contains(newUser.Email))
            throw new ArgumentException("User with this Email allready exists", newUser.Email);
    }
    private void EnsurePositiveAge(CreateUserDTO newUser)
    {
        if (newUser.Age < 0)
            throw new ArgumentException("Age should be positive");
    }
}