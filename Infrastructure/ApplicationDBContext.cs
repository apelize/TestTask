using Entities;
using Microsoft.EntityFrameworkCore;

#pragma warning disable 1591

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
                .Entity<Role>()
                .Property(r => r.Id)
                .UseIdentityColumn(1, 1);

        modelBuilder
                .Entity<Role>()
                .HasData(new Role { Id = 1, Access = "SuperAdmin" },
                         new Role { Id = 2, Access = "Support" },
                         new Role { Id = 3, Access = "Admin" },
                         new Role { Id = 4, Access = "User" });
    }
}