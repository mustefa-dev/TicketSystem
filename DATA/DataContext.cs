using Microsoft.EntityFrameworkCore;
using OneSignalApi.Model;
using TicketSystem.Entities;

namespace TicketSystem.DATA;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }


    public DbSet<AppUser> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }


    public DbSet<Article> Articles { get; set; }

    // here to add
public DbSet<SolverData> SolverDatas { get; set; }
public DbSet<Notifications> Notifications { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Garage> Garages { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Governorate> Governorates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });

        modelBuilder.Entity<Role>().HasData(new Role
        {
            Id = Guid.Parse("f4832aba-9784-4f7f-ace8-b5b251fa4322") ,
            Name = "User"
        });

            
    }


    public virtual async Task<int> SaveChangesAsync(Guid? userId = null)
    {
        // await OnBeforeSaveChanges(userId);
        var result = await base.SaveChangesAsync();
        return result;
    }
}
