using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

public class ApplicationDBContext : DbContext
{
    private readonly IConfiguration _configuration;
    public ApplicationDBContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DatabaseConnection"));
    }
}