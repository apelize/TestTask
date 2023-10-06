using Entities;
using Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using DTO;
using Extensions;

namespace Repositories;

public class UserRepository : IRepository<User>, IUserRepository
{
    protected readonly ApplicationDBContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(ApplicationDBContext context, ILogger<UserRepository> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task AddUser(CreateUserDTO user)
    {
        var newUser = new User
        {
            Name = user.Name,
            Age = user.Age,
            Email = user.Email,
            Status = new List<Role>()
        };

        if (user.RolesId is not null)
            newUser.Status.AddRange(_context.Roles.Where(role => user.RolesId.Contains(role.Id)));
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
    }

    public async Task<User> Get(int id) =>
        (await GetAll()).FirstOrDefault(u => u.Id == id)
            is User userToGet
            ? userToGet
            : throw new ArgumentNullException($"User wasn't found", nameof(userToGet));

    public async Task<IEnumerable<User>> GetAll(string? filter = null, string? filterValue = null, string? sort = null, string order = "ASC", int pageIndex = 1, int pageSize = 10)
    {
        IEnumerable<User> usersToGet = (await _context.Users.Include(u => u.Status).ToListAsync())
                .FilterUsers(filter, filterValue)
                .SortUsers(sort, order);


        return usersToGet.Skip((pageIndex - 1) * pageSize).Take(pageSize);
    }

    public async Task Delete(int id)
    {
        var userToDelete = await _context.Users.FindAsync(id);

        if (userToDelete is null)
            return;

        _context.Users.Remove(userToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<User> Find(Func<User, bool> predicate) =>
        (await GetAll()).Where(predicate).FirstOrDefault()
            is User userToFind
            ? userToFind
            : throw new ArgumentNullException("User wasn't found", nameof(userToFind));


    public async Task Update(UpdateUserDTO user)
    {
        var userToUpdate = await Get(user.id);

        foreach (var property in typeof(UpdateUserDTO).GetProperties().Skip(1).Take(3))
        {
            if (property.GetValue(user) is not null)
                typeof(User)
                    .GetProperty(property.Name.ToString())!
                    .SetValue(userToUpdate, property.GetValue(user));

        }

        if (typeof(UpdateUserDTO).GetProperty("RolesId")!.GetValue(user) is not null)
        {
            IEnumerable<Role> rolesToUpdate = _context.Roles.Where(role => user.RolesId!.Contains(role.Id)).ToList();
            typeof(User).GetProperty("Status")!.SetValue(userToUpdate, rolesToUpdate);
        }

        await _context.SaveChangesAsync();
    }
    public async Task AddRole(int id, int roleId)
    {
        var userToAddRole = await Get(id);
        var roleToAdd = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

        if (roleToAdd is null)
            throw new ArgumentNullException("Role wasn't found", nameof(roleToAdd));

        userToAddRole.Status.Add(roleToAdd);
        await _context.SaveChangesAsync();
    }
}